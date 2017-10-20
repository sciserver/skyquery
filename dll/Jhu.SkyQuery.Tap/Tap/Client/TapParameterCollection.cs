using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Collections;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapParameterCollection : DbParameterCollection, IList<TapParameter>
    {
        private static readonly StringComparer Comparer = StringComparer.InvariantCultureIgnoreCase;

        private object syncRoot = new object();
        private List<TapParameter> parameters;

        #region Properties

        public new TapParameter this[int index]
        {
            get { return parameters[index]; }
            set { parameters[index] = value; }
        }

        public override object SyncRoot
        {
            get { return syncRoot; }
        }

        public override int Count
        {
            get { return parameters.Count; }
        }

        #endregion
        #region Constructors and initializers

        public TapParameterCollection()
        {
            InitializeLifetimeService();
        }

        private void InitializeMembers()
        {
            this.parameters = new List<Client.TapParameter>();
        }

        #endregion

        public override int Add(object value)
        {
            Add((TapParameter)value);
            return parameters.Count;
        }

        public void Add(TapParameter parameter)
        {
            parameters.Add(parameter);
        }

        public override void AddRange(Array values)
        {
            foreach (var i in values)
            {
                parameters.Add((TapParameter)i);
            }
        }

        public override void Clear()
        {
            parameters.Clear();
        }

        public override bool Contains(string value)
        {
            return parameters
                .Where(i => Comparer.Compare(i.ParameterName, value) == 0)
                .Count() > 0;
        }

        public override bool Contains(object value)
        {
            return Contains((TapParameter)value);
        }

        public bool Contains(TapParameter parameter)
        {
            return parameters.Contains(parameter);
        }

        public override void CopyTo(Array array, int index)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                array.SetValue(parameters[i], index + i);
            }
        }

        public void CopyTo(TapParameter[] array, int index)
        {
            CopyTo((Array)array, index);
        }

        public override IEnumerator GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        IEnumerator<TapParameter> IEnumerable<TapParameter>.GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        public override int IndexOf(string parameterName)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                if (Comparer.Compare(parameterName, parameters[i].ParameterName) == 0)
                {
                    return i;
                }
            }

            throw Error.ParameterNotFound(parameterName);
        }

        public override int IndexOf(object value)
        {
            return IndexOf((TapParameter)value);
        }

        public int IndexOf(TapParameter parameter)
        {
            return parameters.IndexOf(parameter);
        }

        public override void Insert(int index, object value)
        {
            Insert(index, (TapParameter)value);
        }

        public void Insert(int index, TapParameter parameter)
        {
            parameters.Insert(index, parameter);
        }

        public override void Remove(object value)
        {
            Remove((TapParameter)value);
        }

        public bool Remove(TapParameter parameter)
        {
            return parameters.Remove(parameter);
        }

        public override void RemoveAt(string parameterName)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                if (Comparer.Compare(parameterName, parameters[i].ParameterName) == 0)
                {
                    parameters.RemoveAt(i);
                    return;
                }
            }

            throw Error.ParameterNotFound(parameterName);
        }

        public override void RemoveAt(int index)
        {
            parameters.RemoveAt(index);
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                if (Comparer.Compare(parameterName, parameters[i].ParameterName) == 0)
                {
                    return parameters[i];
                }
            }

            throw Error.ParameterNotFound(parameterName);
        }

        protected override DbParameter GetParameter(int index)
        {
            return parameters[index];
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                if (Comparer.Compare(parameterName, parameters[i].ParameterName) == 0)
                {
                    parameters[i] = (TapParameter)value;
                    return;
                }
            }

            throw Error.ParameterNotFound(parameterName);
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            SetParameter(index, (TapParameter)value);
        }

        public void SetParameter(int index, TapParameter parameter)
        {
            parameters[index] = parameter;
        }
    }
}
