using System;
using System.Collections.Generic;
using System.Text;

namespace Nelder_Mid
{
    public class Algorithm
    {
        Function function;
        double beta;
        double alfa;
        double gama;
        double eps;
        public Algorithm(Function function, double beta = 2, double alfa = 1, double gama = 0.5, double eps = 0.0001)
        {
            this.function = function;
            this.beta = beta;
            this.alfa = alfa;
            this.gama = gama;
            this.eps = eps;
        }

        double[,] initial_approximation(double[,] vertexes)
        {
            for (int i = 0; i < function.Number_variables + 1; i++)//Инициализация вершин
            {
                for (int j = 0; j < function.Number_variables; j++)
                {
                    if (i == 0)
                    {
                        vertexes[i, j] = 0;
                    }
                    else
                    {
                        if (i - 1 == j)
                        {
                            vertexes[i, j] = 1;
                        }
                        else
                        {
                            vertexes[i, j] = 0;
                        }
                    }
                }
            }
            return vertexes;
        }

        double[] calculation_function(double[,] vertexes)
        {

            double[] function_values = new double[function.Number_variables + 1];
            double[] row = new double[function.Number_variables];
            for (int  i= 0; i < function.Number_variables + 1; i++)//Расчет значений функции в вершина многогранника
            {

                for (int j = 0; j < function.Number_variables; j++)
                    row[j] = vertexes[i, j];
                function_values[i] = function.F(row);
            }
            for (int i = 0; i < function.Number_variables + 1; i++)
            {
                for (int j = 0; j < function.Number_variables; j++)
                {
                    Console.Write("{0:F4}", vertexes[i, j]);
                    Console.Write(" ");

                }
                Console.Write("Значение функции - ");
                Console.Write("{0:F4}", function_values[i]);
                Console.WriteLine();
            }
            Console.WriteLine(new string('_', 50));

            return function_values;
        }
        (int, int) search_extremes(double[] function_values)
        {//поиск макс и мин значения индексов
            int max_ind = 0, min_ind = 0;
            for (int j = 0; j < function.Number_variables + 1; j++)
            {
                if (function_values[j] > function_values[max_ind])
                {
                    max_ind = j;
                }
                if (function_values[j] < function_values[min_ind])
                {
                    min_ind = j;
                }
            }
            return (max_ind, min_ind);
        }
        double[] center_gravity(double[,] vertexes, int max_ind)//поиск центров тяжести
        {
            double[] centers_gravity = new double[function.Number_variables];
            for (int k = 0; k < function.Number_variables; k++)
            {
                double SumP = 0;
                for (int j = 0; j < function.Number_variables + 1; j++)
                {
                    SumP = SumP + vertexes[j, k];
                }
                centers_gravity[k] = (1.0 / function.Number_variables) * (SumP - vertexes[max_ind, k]);

            }
            return centers_gravity;
        }
        bool deviation(double[] centers_gravity, double[] function_values)//проверка погрешности
        {

            double f_centers_gravity = function.F(centers_gravity);

            double sum_squares = 0;
            for (int j = 0; j < function.Number_variables + 1; j++)
            {
                sum_squares += Math.Pow(function_values[j] - f_centers_gravity, 2);
            }
            double error = Math.Sqrt((1.0 / (function.Number_variables + 1)) * sum_squares);

            if (error <= eps)
            {
                return true;//точность достигнута
            }
            else
                return false;//точность не достигнута
        }

        double[] reflection(double[,] vertexes, double[] centers_gravity, int max_ind)//отражение
        {
            double[] vertices_reflection = new double[function.Number_variables];
            for (int i = 0; i < function.Number_variables ; i++)
            {
                vertices_reflection[i] = centers_gravity[i] + alfa * (centers_gravity[i] - vertexes[max_ind, i]);
            }
            return vertices_reflection;
        }

        void stretching(double[] centers_gravity, double[] vertices_reflection, double[,] vertexes, int max_ind)//Растяжение
        {


            double[] vertices_stretching = new double[function.Number_variables];
            Console.WriteLine("Выполнено растяжение");
            for (int i = 0; i < function.Number_variables; i++)
            {
                vertices_stretching[i] = centers_gravity[i] + gama * (vertices_reflection[i] - centers_gravity[i]);
            }
            if (function.F(vertices_stretching) < function.F(vertices_reflection))//Проверка растяжения
            {

                for (int i = 0; i < function.Number_variables; i++)
                {
                    vertexes[max_ind, i] = vertices_stretching[i];
                }
            }
            else
            {

                for (int i = 0; i < function.Number_variables; i++)
                {
                    vertexes[max_ind, i] = vertices_reflection[i];
                }
            }
        }

        void compression(double[] centers_gravity, double[] vertices_reflection, double[,] vertexes, double[] F, int max_ind, int min_ind)//сжатие
        {

            bool check = true;
            double f_vertices = function.F(vertices_reflection);
            double[] vertices_compression = new double[function.Number_variables];
            for (int j = 0; j < function.Number_variables + 1; j++)//Проверка
            {
                if (j != max_ind)
                {
                    if (f_vertices < F[j])
                    {
                        check = false;
                        break;
                    }
                }
            }
            Console.WriteLine("Выполнено сжатие");
            if (check)
            {

                if (f_vertices < F[max_ind])
                {

                    for (int k = 0; k < function.Number_variables; k++)
                    {
                        vertexes[max_ind, k] = vertices_reflection[k];
                    }
                }

                for (int k = 0; k < function.Number_variables ; k++)//Сжатие
                {
                    vertices_compression[k] = centers_gravity[k] + gama * (vertexes[max_ind, k] - centers_gravity[k]);
                }
                if (function.F(vertices_compression) < F[max_ind])
                {

                    for (int k = 0; k < function.Number_variables; k++)
                    {
                        vertexes[max_ind, k] = vertices_compression[k];
                    }
                }
                else
                {

                    for (int j = 0; j < function.Number_variables + 1; j++)
                    {
                        for (int k = 0; k < function.Number_variables; k++)
                        {
                            if (j != min_ind)
                            {
                                vertexes[j, k] = vertexes[min_ind, k] + gama * (vertexes[j, k] - vertexes[min_ind, k]);
                            }
                        }
                    }
                }
            }
            else
            {

                for (int k = 0; k < function.Number_variables; k++)
                {
                    vertexes[max_ind, k] = vertices_reflection[k];
                }
            }
        }

        public double[] algorithm_result()
        {
            double[,] vertexes = new double[function.Number_variables + 1, function.Number_variables];
            double[] function_values;
            var (max_ind, min_ind) = (0, 0);
            double[] centers_gravity;
            double[] vertices_reflection;
            //double[] E;
            vertexes = initial_approximation(vertexes);

            while (true)
            {
                function_values = calculation_function(vertexes);
                (max_ind, min_ind) = search_extremes(function_values);
                centers_gravity = center_gravity(vertexes, max_ind);
                if (deviation(centers_gravity, function_values))
                    break;
                else
                {
                    vertices_reflection = reflection(vertexes, centers_gravity, max_ind);
                    double FR = function.F(vertices_reflection);
                    if (FR < function_values[min_ind])
                    {
                        stretching(centers_gravity, vertices_reflection, vertexes, max_ind);
                    }
                    else
                    {
                        compression(centers_gravity, vertices_reflection, vertexes, function_values, max_ind, min_ind);
                    }

                }
            }
            for (int i=0; i<function.Number_variables; i++)
            {
                Console.WriteLine($"x[{i}] = "+ centers_gravity[i].ToString());
            }
            
            double res = function.F(centers_gravity);
            Console.WriteLine("result = " + res);
            return centers_gravity;
        }
    }
}
