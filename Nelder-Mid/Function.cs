using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
//using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;


namespace Nelder_Mid
{
    public class Function
    {
        string function;
        int number_variables;
        public int Number_variables
        {
            get { return number_variables; }
            set { number_variables = value; }
        }
        public Function(string function)
        {
            int ind = 0;
            this.function = function;
            var regex = new Regex("x\\[(:?[0-9]+)\\]", RegexOptions.Compiled);
            foreach (Match match in regex.Matches(function))
            {
                var i = int.Parse(match.Groups[1].Value);
                if (i > ind)
                    ind = i;
            }
            number_variables = ind+1;
        }
        public double F(double[] x)
        {
            var function_set_values = SetValues(x);
            return Eval(function_set_values);
        }
        string SetValues(double[] array)
        {
            var result = function;
            var regex = new Regex("x\\[(:?[0-9]+)\\]", RegexOptions.Compiled);
            foreach (Match match in regex.Matches(function))
            {
                var ind = int.Parse(match.Groups[1].Value);
                result = result.Replace(match.Value, array[ind].ToString(CultureInfo.InvariantCulture));
            }
            return result;
        }
        double Eval(string input)
        {
            
            Expression e = new Expression(input);
            return e.calculate();
            //return Convert.ToDouble(new DataTable().Compute(input, null));
            
        }
    }
}
