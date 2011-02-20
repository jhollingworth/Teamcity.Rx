using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Teamcity.Rx.Events
{
    [DataContract(Name = "BuildUpdated")]
    public class BuildUpdated : ITeamcityEvent
    {
        private readonly Dictionary<string, object> _updatedValues = new Dictionary<string, object>();
        private readonly string _buildId;

        [DataMember]
        public Dictionary<string, object> UpdatedValues
        {
            get { return _updatedValues; }
        }

        [DataMember]
        public string BuildId
        {
            get { return _buildId; }
        }

        [DataMember]
        public string EventId
        {
            get { return "ub"; }
        }

        public BuildUpdated(Build entity, IEnumerable<Expression<Func<Build, object>>> updatedValues)
        {
            _buildId = entity.Id;

            foreach (var updatedValue in updatedValues)
            {
                _updatedValues[GetMemberInfo(updatedValue).Member.Name] = updatedValue.Compile().Invoke(entity);
            }
        }

        public override string ToString()
        {
            var values = UpdatedValues.Select((prop, value) => string.Format("{0} -> {1}", prop, value)).ToArray(); 

            return string.Format("Build updated. Updated values\n{0}", string.Join("\t", values));
        }

        private static MemberExpression GetMemberInfo(LambdaExpression lambda)
        {
            if (lambda == null)
            {
                throw new ArgumentNullException("lambda", "Expression must be a lambda expression");
            }

            switch (lambda.Body.NodeType)
            {
                case ExpressionType.Convert:
                    return ((UnaryExpression)lambda.Body).Operand as MemberExpression;
                case ExpressionType.MemberAccess:
                    return lambda.Body as MemberExpression;
                default:
                    throw new ArgumentException("Expression type must be convert or memberaccess", "lambda");    
            }
        }
    }
}