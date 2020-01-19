using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SqlGenerator.Core
{
    public class MergeSqlCommand : ISqlCommand
    {

        private List<Tuple<TypeMatchedNotMatched, TypeCommand, SqlCompare, ISqlCommand>> matchedNotMatchedArray;
        private string target;
        private SelectSqlCommand source;
        private SqlColumn[] targetColumns;
        private SqlCompare on;

        enum TypeMatchedNotMatched {
            BySource,
            ByTarget,
            Matched,
        }

        private enum TypeCommand {
            Update,
            Insert,
            Delete
        }

        public MergeSqlCommand(string target,  SqlColumn[] targetColumns, SelectSqlCommand source, SqlCompare on)
        {
            this.target = target;
            this.source = source;
            this.targetColumns = targetColumns;
            this.on = on;
            matchedNotMatchedArray = new List<Tuple<TypeMatchedNotMatched, TypeCommand, SqlCompare, ISqlCommand>>();
        }
        public string getRawCommand()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("MERGE {0} as TARGET", target);
            sb.AppendFormat(" USING ({0}) as SOURCE ({1}) on ({2})", source.getRawCommand(), string.Join(",",targetColumns.Select(p=>p.getRawCommand())), on.getRawCommand());
            foreach(var matchedNotMatched in matchedNotMatchedArray) {
                sb.AppendFormat(" {0}", parseMatchedNotMatched(matchedNotMatched));
            }
            return sb.ToString();
        }

        private string parseMatchedNotMatched(Tuple<TypeMatchedNotMatched, TypeCommand, SqlCompare, ISqlCommand> matchedNotMatched)
        {
            StringBuilder ret = new  StringBuilder();
            ret.AppendFormat("WHEN {0}", matchedNotMatched.Item1 == TypeMatchedNotMatched.Matched ? "MATCHED" : "NOT MATCHED");
            if(matchedNotMatched.Item1 != TypeMatchedNotMatched.Matched ) {
                ret.AppendFormat(" NOT MATCHED BY {0}", matchedNotMatched.Item1 == TypeMatchedNotMatched.BySource ? "SOURCE" : "TARGET" );
            }

            if(matchedNotMatched.Item3 != null) 
                ret.AppendFormat( " AND {0}", matchedNotMatched.Item3.getRawCommand());

            switch(matchedNotMatched.Item2) {
                case TypeCommand.Insert:
                case TypeCommand.Update:
                {
                    ret.AppendFormat(" THEN {0}",matchedNotMatched.Item4.getRawCommand());
                    break;
                }
                case TypeCommand.Delete:
                {
                    ret.AppendFormat(" THEN {0}", "DELETE");
                    break;
                } 
                default:
                    throw new InvalidEnumArgumentException($"Value is not valid: {matchedNotMatched.Item2}");

            }
            return ret.ToString();
        }

        public MergeSqlCommand whenMatched(UpdateSqlCommand updateSqlCommand, SqlCompare andWhere = null) 
        {
            matchedNotMatchedArray.Add(new Tuple<TypeMatchedNotMatched, TypeCommand, SqlCompare, ISqlCommand>(TypeMatchedNotMatched.Matched, TypeCommand.Update, andWhere, updateSqlCommand));
            return this;
        }

        public MergeSqlCommand whenMatchedDelete(SqlCompare andWhere = null) 
        {
            matchedNotMatchedArray.Add(new Tuple<TypeMatchedNotMatched, TypeCommand, SqlCompare, ISqlCommand>(TypeMatchedNotMatched.Matched, TypeCommand.Delete, andWhere, null));
            return this;
        }

        public MergeSqlCommand whenNotMatchedByTarget(MergeInsertSqlCommand insertSqlCommand, SqlCompare andWhere = null)
        {
            matchedNotMatchedArray.Add(new Tuple<TypeMatchedNotMatched, TypeCommand, SqlCompare, ISqlCommand>(TypeMatchedNotMatched.ByTarget, TypeCommand.Insert, andWhere, insertSqlCommand));
            return this;
        }
        public MergeSqlCommand whenNotMatchedBySourceUpdate(UpdateSqlCommand updateSqlCommand, SqlCompare andWhere = null)
        {
            matchedNotMatchedArray.Add(new Tuple<TypeMatchedNotMatched, TypeCommand, SqlCompare, ISqlCommand>(TypeMatchedNotMatched.BySource, TypeCommand.Update, andWhere, updateSqlCommand));
            return this;
        }
        public MergeSqlCommand whenNotMatchedBySourceDelete(SqlCompare andWhere = null)
        {
            matchedNotMatchedArray.Add(new Tuple<TypeMatchedNotMatched, TypeCommand, SqlCompare, ISqlCommand>(TypeMatchedNotMatched.BySource, TypeCommand.Delete, andWhere, null));
            return this;
        }
    }
}
