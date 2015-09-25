namespace IMPression
{
    internal class Variable : EquationValue
    {
        public Variable(string src, int start, EquationElement root)
        {
            var varname = GetVarName(src, start, root);
            Name = varname;
            if (varname.Length > 0)
                m_sb.Append(varname);
        }

        public override bool Signed
        {
            get { return m_signed; }
            set { m_signed = value; }
        }

        public string Name { get; set; }


        public override Complex Value
        {
            get
            {
                var value = Root.GetVar(m_sb.ToString());
                if (Signed) return -value;
                return value;
            }
        }

        private static string GetVarName(string equation, int index, EquationElement root)
        {
            var varname = Term.ExtractName(equation, index).ToLower();
            if (root != null && root.VarExist(varname))
                return varname;
            var ch = equation[index];
            if (ch >= 'a' && ch <= 'z')
                return new string(new[] {ch});
            if (ch >= 'A' && ch <= 'Z')
                return new string(new[] {ch});
            return string.Empty;
        }

        public static bool IsValidDigit(string equation, int index, EquationElement root)
            => GetVarName(equation, index, root).Length > 0;

        public override bool IsValid(string equation, int index) => true;

        public bool IsValid(string equation, int index, EquationElement root)
            => GetVarName(equation, index, root).Length > 0;
    }
}