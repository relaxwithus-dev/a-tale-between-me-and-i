using System;

namespace ATBMI.Dialogue
{
    [Serializable]
    public class NumericCondition
    {
        public NumericConditionType type;
        public ComparisonOperator comparison;
        public int value;
    }

    public enum NumericConditionType
    {
        VisitedCount
        // Tambahkan lainnya jika perlu
    }

    public enum ComparisonOperator
    {
        Equal,
        GreaterThan,
        LessThan
    }
}
