using System.Collections.Generic;
using System.Text;

namespace IMPression
{
    public abstract class EquationElement
    {
        protected StringBuilder m_sb = new StringBuilder();
        private Dictionary<string, bool> m_usedVars;
        protected Dictionary<string, Complex> m_variables;
        public EquationElement Parent { get; set; }
        public virtual int Length => m_sb.Length;

        protected EquationElement Root
        {
            get
            {
                if (Parent == null)
                    return this;
                var parent = Parent;
                while (parent?.Parent != null)
                    parent = parent.Parent;
                return parent;
            }
        }

        protected virtual void Add(char ch)
        {
            m_sb.Append(ch);
        }

        protected virtual void Add(string ch)
        {
            m_sb.Append(ch);
        }

        public abstract bool IsValid(string equation, int index);

        public override string ToString()
        {
            return m_sb.ToString();
        }

        public void ClearVars()
        {
            var root = Root;
            root.m_variables?.Clear();
        }

        public bool VarExist(string varname)
        {
            var root = Root;
            return (root.m_variables != null && root.m_variables.ContainsKey(varname.ToLower()));
        }

        public void SetVar(string varname, string value)
        {
            var term = new Term();
            term.Parse(value);
            SetVar(varname, term.Value);
        }

        public void SetVar(string varname, Complex value)
        {
            var root = Root;
            if (root.m_variables == null)
                root.m_variables = new Dictionary<string, Complex>();
            root.m_variables[varname.ToLower()] = value;
        }

        public Complex GetVar(string varname)
        {
            var root = Root;
            Complex result = 0;
            if (root.m_variables == null || root.m_variables.TryGetValue(varname.ToLower(), out result) == false)
                throw new ParseException(varname, 0, $"La variable '{varname}' n'est pas définie");
            if (m_usedVars == null)
                m_usedVars = new Dictionary<string, bool>();
            if (m_usedVars.ContainsKey(varname) == false)
                m_usedVars[varname] = true;
            return result;
        }

        public string[] GetUsedVars()
        {
            if (m_usedVars == null)
                return new string[0];
            var l = new List<string>(m_usedVars.Keys);
            l.Sort();
            return l.ToArray();
        }
    }
}