using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DLEPCalcul
{
    public static class Calcul
    {
        private const double ToDeg = 180 / Math.PI;
        private const double ToRad = Math.PI / 180;
        private static double Dsr(InitialData data) => Math.Pow(2, (double)1 / 3) * data.D_phase;
        private static double R_pr(InitialData data) => Math.Sqrt(3 * (data.F_st + data.F_al) / Math.PI);
        private static double X0_single(InitialData data) => 0.144 * Math.Log10(Dsr(data) * 1000 / R_pr(data)) + 0.0157;
        private static double B0_single(InitialData data) => 7.58 / Math.Log10(Dsr(data) * 1000 / R_pr(data)) * Math.Pow(10, -6);
        private static Complex Z0_single(InitialData data) => new Complex(data.R0, X0_single(data));
        private static Complex Y0_single(InitialData data) => new Complex(0, B0_single(data));
        private static Complex Gamma0_single(InitialData data) => Complex.Sqrt(Complex.Multiply(Z0_single(data), Y0_single(data)));
        private static double Alfa0_single(InitialData data) => Gamma0_single(data).Real;
        private static double Beta0_rad_single(InitialData data) => Gamma0_single(data).Magnitude * Math.Sin(Gamma0_single(data).Phase);
        private static double Beta0_deg_single(InitialData data) => Beta0_rad_single(data) * 180 / Math.PI;
        private static Complex Zc_single(InitialData data) => Complex.Sqrt(Complex.Divide(Z0_single(data), Y0_single(data)));
        private static double Pc_single(InitialData data) => Math.Pow(data.VoltNom, 2) / Zc_single(data).Magnitude;

        public static void Refresh(InitialData data)
        {
            #region ParamCollection
            ParamCollection[0].Value1 = new Complex(Dsr(data), 0);

            ParamCollection[1].Value1 = new Complex(R_pr(data), 0);
            ParamCollection[1].Value2 = new Complex(R_eq(data), 0);

            ParamCollection[2].Value1 = new Complex(X0_single(data), 0);
            ParamCollection[2].Value2 = new Complex(X0(data), 0);

            ParamCollection[3].Value1 = new Complex(B0_single(data), 0);
            ParamCollection[3].Value2 = new Complex(B0(data), 0);

            ParamCollection[4].Value1 = Z0_single(data);
            ParamCollection[4].Value2 = Z0(data);

            ParamCollection[5].Value1 = Y0_single(data);
            ParamCollection[5].Value2 = Y0(data);

            ParamCollection[6].Value1 = Gamma0_single(data);
            ParamCollection[6].Value2 = Gamma0(data);

            ParamCollection[7].Value1 = Alfa0_single(data);
            ParamCollection[7].Value2 = Alfa0(data);

            ParamCollection[8].Value1 = Beta0_deg_single(data);
            ParamCollection[8].Value2 = Beta0_deg(data);

            ParamCollection[9].Value1 = Zc_single(data).Magnitude;
            ParamCollection[9].Value2 = Zc(data).Magnitude;

            ParamCollection[10].Value1 = Pc_single(data);
            ParamCollection[10].Value2 = Pc(data);
            #endregion

            #region FourPoleCollection
            var nData = new InitialData(data, 0);

            FourPoleCollection[0].Value1 = A(data);
            FourPoleCollection[0].Value2 = A(nData);

            FourPoleCollection[1].Value1 = B(data);
            FourPoleCollection[1].Value2 = B(nData);

            FourPoleCollection[2].Value1 = C(data);
            FourPoleCollection[2].Value2 = C(nData);

            FourPoleCollection[3].Value1 = D(data);
            FourPoleCollection[3].Value2 = D(nData);
            #endregion

            #region Coeff1Collection

            Coeff1Collection[0].Value1 = new Complex(Kr_half(data), 0);
            Coeff1Collection[1].Value1 = new Complex(Kx_half(data), 0);
            Coeff1Collection[2].Value1 = new Complex(Kb_half(data), 0);
            Coeff1Collection[3].Value1 = new Complex(R_cell_half(data), 0);
            Coeff1Collection[4].Value1 = new Complex(X_cell_half(data), 0);
            Coeff1Collection[5].Value1 = new Complex(B_cell_half(data), 0);
            Coeff1Collection[6].Value1 = new Complex(B_r(data), 0);
            Coeff1Collection[7].Value1 = new Complex(Q_reactor(data), 0);

            #endregion

            #region Coeff2Collection

            Coeff2Collection[0].Value1 = new Complex(Kr(data), 0);
            Coeff2Collection[1].Value1 = new Complex(Kx(data), 0);
            Coeff2Collection[2].Value1 = new Complex(Kb(data), 0);
            Coeff2Collection[3].Value1 = new Complex(R_cell(data), 0);
            Coeff2Collection[4].Value1 = new Complex(X_cell(data), 0);
            Coeff2Collection[5].Value1 = new Complex(B_cell(data), 0);

            #endregion

            #region UCollection

            UCollection[0].Value1 = Get_U_Nat(data);
            UCollection[1].Value1 = Get_U_More(data, data.Length / 2);
            UCollection[2].Value1 = Get_U_Less(data, data.Length / 2);
            UCollection[3].Value1 = Get_U_XX(data, data.Length / 2);

            #endregion


        }


        #region Параметры

        private static double R_eq(InitialData data) => Math.Pow(data.D_wire / 20 * data.A_splitwires * data.A_splitwires, 1.0 / 3.0);
        private static double X0(InitialData data) => 0.144 * Math.Log10(Dsr(data) * 100 / R_eq(data)) + 0.0157 / data.N_split;
        private static double B0(InitialData data) => 7.58 / Math.Log10(Dsr(data) * 100 / R_eq(data)) * Math.Pow(10, -6);
        private static Complex Z0(InitialData data) => new Complex(data.R0, X0(data));
        private static Complex Y0(InitialData data) => new Complex(0, B0(data));
        private static Complex Gamma0(InitialData data) => Complex.Sqrt(Complex.Multiply(Z0(data), Y0(data)));
        private static double Alfa0(InitialData data) => Gamma0(data).Real;
        private static double Beta0_rad(InitialData data) => Gamma0(data).Magnitude * Math.Sin(Gamma0(data).Phase);
        private static double Beta0_deg(InitialData data) => Beta0_rad(data) * 180 / Math.PI;
        private static Complex Zc(InitialData data) => Complex.Sqrt(Complex.Divide(Z0(data), Y0(data)));
        private static double Pc(InitialData data) => Math.Pow(data.VoltNom, 2) / Zc(data).Magnitude;

        public static ObservableCollection<RowShell> ParamCollection = new ObservableCollection<RowShell>()
        {
            new RowShell()
            {
                Name = "Среднегеометрическое растояние между проводами фаз, м",
            },
            new RowShell()
            {
                Name = "Эквивалентный радиус провода, мм",
            },
            new RowShell()
            {
                Name = "Индуктивное сопротивление, Ом/км",
            },
            new RowShell()
            {
                Name = "Емкостная проводимость, См/км",
            },
            new RowShell()
            {
                Name = "Комплексное сопротивление, Ом/км",
            },
            new RowShell()
            {
                Name = "Полная проводимость, См/км",
            },
            new RowShell()
            {
                Name = "Коэффициент распространения волны, град/км",
            },
            new RowShell()
            {
                Name = "Коэффициент затухания, 1/км",
            },
            new RowShell()
            {
                Name = "Коэффициент фазы, град/км",
            },
            new RowShell()
            {
                Name = "Волновое сопротивление, Ом",
            },
            new RowShell()
            {
                Name = "Натуральная мощность, МВт",
            }
        };

        #endregion

        #region Четырёхполюсник

        private static Complex A(InitialData data) => Complex.Cosh(Complex.Multiply(Gamma0(data), data.Length));
        private static Complex B(InitialData data) => Complex.Multiply(Zc(data), Complex.Sinh(Complex.Multiply(Gamma0(data), data.Length)));
        private static Complex C(InitialData data) => Complex.Divide(Complex.Sinh(Complex.Multiply(Gamma0(data), data.Length)), Zc(data));
        private static Complex D(InitialData data) => A(data);

        public static ObservableCollection<RowShell> FourPoleCollection = new ObservableCollection<RowShell>()
        {
            new RowShell()
            {
                Name = "A",
            },
            new RowShell()
            {
                Name = "B, Ом",
            },
            new RowShell()
            {
                Name = "C, См",
            },
            new RowShell()
            {
                Name = "D",
            },
        };

        #endregion

        #region Поправочные коэффициенты

        private static double Kr_half(InitialData data) => 1 - data.Length / 2 * data.Length / 2 / 3 * X0(data) * B0(data);
        private static double Kx_half(InitialData data) => 1 - 0.25 * data.Length * data.Length * (X0(data) * B0(data) - data.R0 * data.R0 * B0(data) / X0(data)) / 6;
        private static double Kb_half(InitialData data) => 0.5 * (3 + Kr_half(data)) / (1 + Kr_half(data));
        private static double R_cell_half(InitialData data) => data.R0 * data.Length / 2 * Kr_half(data);
        private static double X_cell_half(InitialData data) => X0(data) * data.Length / 2 * Kx_half(data);
        private static double B_cell_half(InitialData data) => B0(data) / 2 * data.Length / 2 * Kb_half(data);
        private static double B_r(InitialData data) => 2 * B_cell_half(data) / 3;
        private static double Q_reactor(InitialData data) => data.VoltNom * data.VoltNom * B_r(data);

        public static ObservableCollection<RowShell> Coeff1Collection = new ObservableCollection<RowShell>()
        {
            new RowShell()
            {
                Name = "По активному сопротивлению",
            },
            new RowShell()
            {
                Name = "По индуктивному сопротивлению",
            },
            new RowShell()
            {
                Name = "По ёмкостному сопротивлению",
            },
            new RowShell()
            {
                Name = "Активное сопротивление схемы замещения",
            },
            new RowShell()
            {
                Name = "Индуктивное сопротивление схемы замещения",
            },
            new RowShell()
            {
                Name = "Емкостное сопротивление схемы замещения",
            },
            new RowShell()
            {
                Name = "Суммарная проводимость компенсирующих реакторов",
            },
            new RowShell()
            {
                Name = "Номинальная мощность реактора",
            },
        };

        public static ObservableCollection<RowShell> Coeff2Collection = new ObservableCollection<RowShell>()
        {
            new RowShell()
                {
                    Name = "По активному сопротивлению",
                },
                new RowShell()
                {
                    Name = "По индуктивному сопротивлению",
                },
                new RowShell()
                {
                    Name = "По ёмкостному сопротивлению",
                },
                new RowShell()
                {
                    Name = "Активное сопротивление схемы замещения",
                },
                new RowShell()
                {
                    Name = "Индуктивное сопротивление схемы замещения",
                },
                new RowShell()
                {
                    Name = "Емкостное сопротивление схемы замещения",
                },
        };

        private static double Kr(InitialData data) => 1 - data.Length * data.Length * X0(data) * B0(data) / 3;
        private static double Kx(InitialData data) => 1 - data.Length * data.Length * (X0(data) * B0(data) - data.R0 * data.R0 * B0(data) / X0(data)) / 6;
        private static double Kb(InitialData data) => 0.5 * (3 + Kr(data)) / (1 + Kr(data));
        private static double R_cell(InitialData data) => data.R0 * data.Length * Kr(data);
        private static double X_cell(InitialData data) => X0(data) * data.Length * Kx(data);
        private static double B_cell(InitialData data) => B0(data) * data.Length * Kb(data) / 2;


        #endregion

        #region Реактор

        static readonly double Areactor = 1;
        static readonly double Breactor = 0;
        static readonly double Dreactor = 1;

        private static double A1(InitialData data) => Math.Cos(Beta0_rad(data) * data.Length / 2);
        private static double A2(InitialData data) => A1(data);
        private static double D1(InitialData data) => A1(data);
        private static double D2(InitialData data) => A1(data);
        private static Complex B1(InitialData data) => new Complex(0, Zc(data).Magnitude * Math.Sin(Beta0_rad(data) * data.Length / 2));
        private static Complex B2(InitialData data) => B1(data);
        private static Complex C1(InitialData data) => new Complex(0, Math.Sin(Beta0_rad(data) * data.Length / 2) / Zc(data).Magnitude);
        private static Complex C2(InitialData data) => C1(data);

        public static (double[] Xs, double[] Ys, Complex[] cplx) A_react(InitialData data, int start, int end, int step)
        {
            double[] xs = new double[(end - start) / step];
            double[] ys = new double[(end - start) / step];
            Complex[] cplxs = new Complex[(end - start) / step];

            for (int i = start; i < end; i += step)
            {
                cplxs[i] = Get_A_react(data, i);
                xs[i] = i;
                ys[i] = cplxs[i].Magnitude;
            }
            return (xs, ys, cplxs);
        }
        private static Complex Get_A_react(InitialData data, int value)
        {
            Complex Creactor = new Complex(0, -value * B_r(data));
            Complex A_ = A1(data) * Areactor * A2(data) + A1(data) * Breactor * C2(data) + B1(data) * Creactor * A2(data) + B1(data) * Dreactor * C2(data);
            return A_;
        }

        public static (double[] Xs, double[] Ys, Complex[] cplx) B_react(InitialData data, int start, int end, int step)
        {
            double[] xs = new double[(end - start) / step];
            double[] ys = new double[(end - start) / step];
            Complex[] cplxs = new Complex[(end - start) / step];

            for (int i = start; i < end; i += step)
            {
                cplxs[i] = Get_B_react(data, i);
                xs[i] = i;
                ys[i] = cplxs[i].Magnitude;
            }
            return (xs, ys, cplxs);
        }

        private static Complex Get_B_react(InitialData data, int value)
        {
            Complex Creactor = new Complex(0, -value * B_r(data));
            Complex B_ = A1(data) * Areactor * B2(data) + A1(data) * Breactor * D2(data) + B1(data) * Creactor * B2(data) + B1(data) * Dreactor * D2(data);
            return B_;
        }

        public static (double[] Xs, double[] Ys, Complex[] cplx) C_react(InitialData data, int start, int end, int step)
        {
            double[] xs = new double[(end - start) / step];
            double[] ys = new double[(end - start) / step];
            Complex[] cplxs = new Complex[(end - start) / step];

            for (int i = start; i < end; i += step)
            {
                cplxs[i] = Get_C_react(data, i);
                xs[i] = i;
                ys[i] = cplxs[i].Magnitude;
            }
            return (xs, ys, cplxs);
        }
        private static Complex Get_C_react(InitialData data, int value)
        {
            Complex Creactor = new Complex(0, -value * B_r(data));
            Complex C_ = C1(data) * Areactor * A2(data) + C1(data) * Breactor * C2(data) + D1(data) * Creactor * A2(data) + D1(data) * Dreactor * C2(data);
            return C_;
        }

        public static (double[] Xs, double[] Ys, Complex[] cplx) D_react(InitialData data, int start, int end, int step)
        {
            double[] xs = new double[(end - start) / step];
            double[] ys = new double[(end - start) / step];
            Complex[] cplxs = new Complex[(end - start) / step];

            for (int i = start; i < end; i += step)
            {
                cplxs[i] = Get_D_react(data, i);
                xs[i] = i;
                ys[i] = cplxs[i].Magnitude;
            }
            return (xs, ys, cplxs);
        }
        private static Complex Get_D_react(InitialData data, int value)
        {
            Complex Creactor = new Complex(0, -value * B_r(data));
            Complex D_ = A1(data) * Areactor * A2(data) + A1(data) * Breactor * C2(data) + B1(data) * Creactor * A2(data) + B1(data) * Dreactor * C2(data);
            return D_;
        }

        public static (double[] Xs, double[] Ys) Zv_react(InitialData data, int start, int end, int step)
        {
            double[] xs = new double[(end - start) / step];
            double[] ys = new double[(end - start) / step];
            for (int i = start; i < end; i += step)
            {
                xs[i] = i;
                ys[i] = Math.Sqrt(Get_B_react(data, i).Magnitude / Get_C_react(data, i).Magnitude);
            }
            return (xs, ys);
        }
        private static double Get_Zv_react(InitialData data, int value)
        {
            return Math.Sqrt(Get_B_react(data, value).Magnitude / Get_C_react(data, value).Magnitude);
        }

        public static (double[] Xs, double[] Ys) Alpha_react(InitialData data, int start, int end, int step)
        {
            double[] xs = new double[(end - start) / step];
            double[] ys = new double[(end - start) / step];
            for (int i = start; i < end; i += step)
            {
                xs[i] = i;
                ys[i] = Math.Atan(Math.Sqrt(Get_B_react(data, i).Magnitude * Get_C_react(data, i).Magnitude / Get_A_react(data, i).Magnitude / Get_D_react(data, i).Magnitude)) * 180 / Math.PI;
            }
            return (xs, ys);
        }

        public static (double[] Xs, double[] Ys) Pc_react(InitialData data, int start, int end, int step)
        {
            double[] xs = new double[(end - start) / step];
            double[] ys = new double[(end - start) / step];
            for (int i = start; i < end; i += step)
            {
                xs[i] = i;
                ys[i] = Math.Pow(data.VoltNom, 2) / Get_Zv_react(data, i);
            }
            return (xs, ys);
        }

        #endregion

        #region УПК

        static readonly double upk_A = 1;
        static readonly double upk_C = 0;
        static readonly double upk_D = 1;

        public static (double[] Xs, double[] Ys, Complex[] cplx) A_upk(InitialData data)
        {
            double[] xs = new double[5];
            double[] ys = new double[5];
            Complex[] cplxs = new Complex[5];

            for (int i = 0; i < 5; i++)
            {
                cplxs[i] = Get_A_upk(data, i * 0.25);
                xs[i] = i * 0.25;
                ys[i] = cplxs[i].Magnitude;
            }
            return (xs, ys, cplxs);
        }
        private static Complex Get_A_upk(InitialData data, double value)
        {
            Complex upk = new Complex(0, -value * X0(data) * data.Length);
            Complex A_ = A1(data) * upk_A * A2(data) + A1(data) * upk * C2(data) + B1(data) * upk_C * A2(data) + B1(data) * upk_D * C2(data);
            return A_;
        }

        public static (double[] Xs, double[] Ys, Complex[] cplx) B_upk(InitialData data)
        {
            double[] xs = new double[5];
            double[] ys = new double[5];
            Complex[] cplxs = new Complex[5];

            for (int i = 0; i < 5; i++)
            {
                cplxs[i] = Get_B_upk(data, i * 0.25);
                xs[i] = i * 0.25;
                ys[i] = cplxs[i].Magnitude;
            }
            return (xs, ys, cplxs);
        }
        private static Complex Get_B_upk(InitialData data, double value)
        {
            Complex upk = new Complex(0, -value * X0(data) * data.Length);
            Complex B_upk = A1(data) * upk_A * B2(data) + A1(data) * upk * D2(data) + B1(data) * upk_C * B2(data) + B1(data) * upk_D * D2(data);
            return B_upk;
        }

        public static (double[] Xs, double[] Ys, Complex[] cplx) C_upk(InitialData data)
        {
            double[] xs = new double[5];
            double[] ys = new double[5];
            Complex[] cplxs = new Complex[5];

            for (int i = 0; i < 5; i++)
            {
                cplxs[i] = Get_C_upk(data, i * 0.25);
                xs[i] = i * 0.25;
                ys[i] = cplxs[i].Magnitude;
            }
            return (xs, ys, cplxs);
        }
        private static Complex Get_C_upk(InitialData data, double value)
        {
            Complex upk = new Complex(0, -value * X0(data) * data.Length);
            Complex C_upk = C1(data) * upk_A * A2(data) + C1(data) * upk * C2(data) + D1(data) * upk_C * A2(data) + D1(data) * upk_D * C2(data);
            return C_upk;
        }

        public static (double[] Xs, double[] Ys, Complex[] cplx) D_upk(InitialData data)
        {
            double[] xs = new double[5];
            double[] ys = new double[5];
            Complex[] cplxs = new Complex[5];

            for (int i = 0; i < 5; i++)
            {
                cplxs[i] = Get_A_upk(data, i * 0.25);
                xs[i] = i * 0.25;
                ys[i] = cplxs[i].Magnitude;
            }
            return (xs, ys, cplxs);
        }

        public static (double[] Xs, double[] Ys) Zv_upk(InitialData data)
        {
            double[] xs = new double[5];
            double[] ys = new double[5];
            for (int i = 0; i < 5; i++)
            {
                xs[i] = i * 0.25;
                ys[i] = Math.Sqrt(Get_B_upk(data, i * 0.25).Magnitude / Get_C_upk(data, i * 0.25).Magnitude);
            }
            return (xs, ys);
        }

        public static (double[] Xs, double[] Ys) Alpha_upk(InitialData data)
        {
            double[] xs = new double[5];
            double[] ys = new double[5];
            for (int i = 0; i < 5; i++)
            {
                xs[i] = i * 0.25;
                ys[i] = Math.Atan(Math.Sqrt(Get_B_upk(data, i * 0.25).Magnitude * Get_C_upk(data, i * 0.25).Magnitude / Get_A_upk(data, i * 0.25).Magnitude / Get_A_upk(data, i * 0.25).Magnitude)) * 180 / Math.PI;
            }
            return (xs, ys);
        }
        public static (double[] Xs, double[] Ys) Pc_upk(InitialData data)
        {
            double[] xs = new double[5];
            double[] ys = new double[5];
            for (int i = 0; i < 5; i++)
            {
                xs[i] = i * 0.25;
                ys[i] = Math.Pow(data.VoltNom, 2) / Math.Sqrt(Get_B_upk(data, i * 0.25).Magnitude / Get_C_upk(data, i * 0.25).Magnitude);
            }
            return (xs, ys);
        }

        #endregion

        #region Данные распределения

        private static double Ctg(double x)
        {
            return 1 / Math.Tan(x);
        }

        private static Complex Ctg(Complex x)
        {
            return 1 / Complex.Tan(x);
        }

        // P<Pc
        private static double P2_less(InitialData data) => data.K1 * Pc(data);
        private static double P2_rel_less(InitialData data) => data.K1;
        private static double Q2_rel_less(InitialData data) => -Ctg(Beta0_rad(data) * data.Length) + Math.Sqrt(1 / Math.Pow(Math.Sin(Beta0_rad(data) * data.Length), 2) - Math.Pow(P2_rel_less(data), 2));
        private static double Q2_less(InitialData data) => Q2_rel_less(data) * Pc(data);
        private static double Φ_rad_less(InitialData data) => Math.Atan(Q2_less(data) / P2_less(data));
        private static Complex I2_less(InitialData data) => new Complex(
            Math.Sqrt(Math.Pow(P2_less(data), 2) + Math.Pow(Q2_less(data), 2)) / data.VoltNom * Math.Cos(-Φ_rad_less(data)),
            Math.Sqrt(Math.Pow(P2_less(data), 2) + Math.Pow(Q2_less(data), 2)) / data.VoltNom * Math.Sin(-Φ_rad_less(data)));

        // P>Pc
        private static double P2_more(InitialData data) => data.K2 * Pc(data);
        private static double P2_rel_more(InitialData data) => data.K2;
        private static Complex Q2_rel_more(InitialData data) => -Ctg(new Complex(Beta0_rad(data), 0) * new Complex(data.Length, 0)) +
            Complex.Sqrt(1 / Complex.Pow(Complex.Sin(new Complex(Beta0_rad(data), 0) * new Complex(data.Length, 0)), 2) - Complex.Pow(new Complex(P2_rel_more(data), 0), 2));
        private static Complex Q2_more(InitialData data) => Q2_rel_more(data) * Pc(data);
        private static double Φ_rad_more(InitialData data) => Math.Atan(Q2_more(data).Magnitude / P2_more(data));
        private static Complex I2_more(InitialData data)
        {
            double P2 = P2_more(data);
            double Q2 = Q2_more(data).Magnitude;
            double deg = Φ_rad_more(data);

            return new Complex(
                       Math.Sqrt(Math.Pow(P2, 2) + Math.Pow(Q2, 2)) / data.VoltNom * Math.Cos(deg),
                       Math.Sqrt(Math.Pow(P2, 2) + Math.Pow(Q2, 2)) / data.VoltNom * Math.Sin(deg));
        }


        #endregion

        #region Напряжение в линии

        public static void UCollectionRefresh(InitialData data, double l)
        {
            UCollection[0].Value2 = Get_U_Nat(data);
            UCollection[1].Value2 = Get_U_More(data, l);
            UCollection[2].Value2 = Get_U_Less(data, l);
            UCollection[3].Value2 = Get_U_XX(data, l);
        }

        public static ObservableCollection<RowShell> UCollection = new ObservableCollection<RowShell>()
        {
            new RowShell()
            {
                Name = "Напряжение при мощности равной натуральной",
            },
            new RowShell()
            {
                Name = "Напряжение при мощности больше натуральной",
            },
            new RowShell()
            {
                Name = "Напряжение при мощности меньше натуральной",
            },
            new RowShell()
            {
                Name = "Напряжение при одностороннем включении",
            },
        };

        // Холосной ход
        public static (double[] Xs, double[] Ys) U_XX(InitialData data)
        {
            double[] xs = new double[(int)data.Length + 1];
            double[] ys = new double[(int)data.Length + 1];

            double Beta = Beta0_rad(data);
            Complex A = Math.Cos(Beta0_rad(data) * data.Length);
            Complex B = Zc(data).Magnitude * Math.Sin(Beta0_rad(data) * data.Length) * Complex.ImaginaryOne;

            Parallel.For(0, (int)data.Length + 1, (i) =>
            {
                xs[i] = i;
                ys[i] = Math.Cos(Beta * (data.Length - i)) *
                ((A * data.VoltNom + B * I2_more(data)).Magnitude / A.Magnitude);
            });

            return (xs, ys);
        }

        public static double Get_U_XX(InitialData data, double l)
        {
            double Beta = Beta0_rad(data);
            Complex A = Math.Cos(Beta * data.Length);
            Complex B = Zc(data).Magnitude * Math.Sin(Beta * data.Length) * Complex.ImaginaryOne;

            return Math.Cos(Beta * (data.Length - l)) *
                ((A * data.VoltNom + B * I2_more(data)).Magnitude / A.Magnitude);
        }

        // Натуральная мощность
        public static (double[] Xs, double[] Ys) U_Nat(InitialData data)
        {
            double[] xs = new double[2];
            double[] ys = new double[2];

            xs[0] = 0;
            xs[1] = data.Length;
            ys[0] = data.VoltNom;
            ys[1] = data.VoltNom;

            return (xs, ys);
        }

        public static double Get_U_Nat(InitialData data)
        {
            return data.VoltNom;
        }

        // Мощность меньше натуральной
        public static (double[] Xs, double[] Ys) U_Less(InitialData data)
        {
            double[] xs = new double[(int)data.Length + 1];
            double[] ys = new double[(int)data.Length + 1];

            Complex I2 = I2_less(data);
            double Beta = Beta0_rad(data);
            double Z = Zc(data).Magnitude;

            Parallel.For(0, (int)data.Length + 1, (i) =>
            {
                xs[i] = i;
                ys[i] = (Math.Cos(Beta * (data.Length - i)) * data.VoltNom +
                Z * Complex.ImaginaryOne * Math.Sin(Beta * (data.Length - i)) * I2).Magnitude;
            });
            return (xs, ys);
        }
        public static double Get_U_Less(InitialData data, double l) => (Math.Cos(Beta0_rad(data) * (data.Length - l)) * data.VoltNom +
                Zc(data).Magnitude * Complex.ImaginaryOne * Math.Sin(Beta0_rad(data) * (data.Length - l)) * I2_less(data)).Magnitude;

        // Мощность больше натуральной
        public static (double[] Xs, double[] Ys) U_More(InitialData data)
        {
            double[] xs = new double[(int)data.Length + 1];
            double[] ys = new double[(int)data.Length + 1];

            double Beta = Beta0_rad(data);
            Complex I2 = I2_more(data);
            double Z = Zc(data).Magnitude;

            Parallel.For(0, (int)data.Length + 1, (i) =>
            {
                xs[i] = i;
                ys[i] = (Math.Cos(Beta * (data.Length - i)) * data.VoltNom +
                Z * Complex.ImaginaryOne * Math.Sin(Beta * (data.Length - i)) * I2).Magnitude;
            });
            return (xs, ys);
        }

        public static double Get_U_More(InitialData data, double l) => (Math.Cos(Beta0_rad(data) * (data.Length - l)) * data.VoltNom +
                Zc(data).Magnitude * Complex.ImaginaryOne * Math.Sin(Beta0_rad(data) * (data.Length - l)) * I2_more(data)).Magnitude;


        #endregion

        #region Поддержание напряжения

        public static void QPCollectionRefresh(InitialData data, double l)
        {
            QPCollection[0].Value1 = Q1_distribution_k(data, l);
            QPCollection[1].Value1 = Q1_distribution_n(data, l);
            QPCollection[2].Value1 = Q2_distribution_n(data, l);
            QPCollection[3].Value1 = Q2_distribution_k(data, l);
        }

        public static ObservableCollection<RowShell> QPCollection = new ObservableCollection<RowShell>()
        {
             new RowShell()
             {
                 Name = "Q1=f(P2)",
             },
             new RowShell()
             {
                 Name = "Q1=f(P1)",
             },
             new RowShell()
             {
                 Name = "Q2=f(P1)",
             },
             new RowShell()
             {
                 Name = "Q2=f(P2)",
             },
        };

        public static (double[] Xs, double[] Ys) Q1_n(InitialData data)
        {
            int count = (int)(Pc(data) + Pc(data) * 0.1) < 3 ?
                3 : (int)(Pc(data) + Pc(data) * 0.15);

            double[] xs = new double[count];
            double[] ys = new double[count];

            Complex _B = B(data);
            Complex _D = D(data);

            Parallel.For(0, count, (i) =>
            {
                xs[i] = i;
                ys[i] = (Complex.Conjugate(_D) * _B).Imaginary * data.VoltNom * data.VoltNom / _B.Magnitude / _B.Magnitude -
                Math.Sqrt(Math.Pow(data.VoltNom * data.VoltNom / _B.Magnitude, 2) - Math.Pow(i - (Complex.Conjugate(D(data)) * _B).Real * data.VoltNom * data.VoltNom / _B.Magnitude / _B.Magnitude, 2)); ;
            });
            return (xs, ys);
        }
        public static double Q1_distribution_n(InitialData data, double P1)
        {
            Complex _B = B(data);
            Complex _D = D(data);

            double Q1 = (Complex.Conjugate(_D) * _B).Imaginary * data.VoltNom * data.VoltNom / _B.Magnitude / _B.Magnitude -
                Math.Sqrt(Math.Pow(data.VoltNom * data.VoltNom / _B.Magnitude, 2) - Math.Pow(P1 - (Complex.Conjugate(_D) * _B).Real * data.VoltNom * data.VoltNom / _B.Magnitude / _B.Magnitude, 2));
            return Q1;
        }


        public static (double[] Xs, double[] Ys) Q2_n(InitialData data)
        {
            int count = (int)(Pc(data) + Pc(data) * 0.1) < 3 ?
                3 : (int)(Pc(data) + Pc(data) * 0.15);

            double[] xs = new double[count];
            double[] ys = new double[count];

            Complex _A = A(data);
            Complex _B = B(data);
            Complex _C = C(data);
            Complex _D = D(data);

            Parallel.For(0, count, (i) =>
            {
                Complex h1 = -_D * Complex.Conjugate(_C) * data.VoltNom * data.VoltNom;
                Complex h2 = -_B * Complex.Conjugate(_A) * (Math.Pow(i, 2) + Math.Pow(Q1_distribution_n(data, i), 2)) / data.VoltNom / data.VoltNom;
                Complex h3 = (_B * Complex.Conjugate(_C) + _D * Complex.Conjugate(_A)) * i;
                Complex h4 = Complex.ImaginaryOne * (-_B * Complex.Conjugate(_C) + _D * Complex.Conjugate(_A)) * Q1_distribution_n(data, i);

                xs[i] = i;
                ys[i] = (h1 + h2 + h3 + h4).Imaginary;
            });
            return (xs, ys);
        }
        public static double Q2_distribution_n(InitialData data, double P1)
        {
            Complex h1 = -D(data) * Complex.Conjugate(C(data)) * data.VoltNom * data.VoltNom;
            Complex h2 = -B(data) * Complex.Conjugate(A(data)) * (Math.Pow(P1, 2) + Math.Pow(Q1_distribution_n(data, P1), 2)) / data.VoltNom / data.VoltNom;
            Complex h3 = (B(data) * Complex.Conjugate(C(data)) + D(data) * Complex.Conjugate(A(data))) * P1;
            Complex h4 = Complex.ImaginaryOne * (-B(data) * Complex.Conjugate(C(data)) + D(data) * Complex.Conjugate(A(data))) * Q1_distribution_n(data, P1);
            double Q2 = (h1 + h2 + h3 + h4).Imaginary;
            return Q2;
        }

        //по данным конца

        public static (double[] Xs, double[] Ys) Q2_k(InitialData data)
        {
            int count = (int)(Pc(data) + Pc(data) * 0.1) < 3 ?
                3 : (int)(Pc(data) + Pc(data) * 0.15);

            double[] xs = new double[count];
            double[] ys = new double[count];

            Complex _A = A(data);
            Complex _B = B(data);

            Parallel.For(0, count, (i) =>
            {
                xs[i] = i;
                ys[i] = -1 * (Complex.Conjugate(_A) * _B).Imaginary * data.VoltNom * data.VoltNom / _B.Magnitude / _B.Magnitude +
                Math.Sqrt(Math.Pow(data.VoltNom * data.VoltNom / _B.Magnitude, 2) - Math.Pow(i + (Complex.Conjugate(_A) * _B).Real * data.VoltNom * data.VoltNom / _B.Magnitude / _B.Magnitude, 2));
            });
            return (xs, ys);
        }
        public static double Q2_distribution_k(InitialData data, double P2)
        {
            double Q2 = -1 * (Complex.Conjugate(A(data)) * B(data)).Imaginary * data.VoltNom * data.VoltNom / B(data).Magnitude / B(data).Magnitude +
                Math.Sqrt(Math.Pow(data.VoltNom * data.VoltNom / B(data).Magnitude, 2) - Math.Pow(P2 + (Complex.Conjugate(A(data)) * B(data)).Real * data.VoltNom * data.VoltNom / B(data).Magnitude / B(data).Magnitude, 2));
            return Q2;
        }


        public static (double[] Xs, double[] Ys) Q1_k(InitialData data)
        {
            int count = (int)(Pc(data) + Pc(data) * 0.1) < 3 ?
                3 : (int)(Pc(data) + Pc(data) * 0.15);

            double[] xs = new double[count];
            double[] ys = new double[count];

            Complex _A = A(data);
            Complex _B = B(data);
            Complex _C = C(data);
            Complex _D = D(data);

            Parallel.For(0, count, (i) =>
            {
                Complex h1 = _A * Complex.Conjugate(_C) * data.VoltNom * data.VoltNom;
                Complex h2 = _B * Complex.Conjugate(_D) * (Math.Pow(i, 2) + Math.Pow(Q2_distribution_k(data, i), 2)) / data.VoltNom / data.VoltNom;
                Complex h3 = (_B * Complex.Conjugate(_C) + A(data) * Complex.Conjugate(_D)) * i;
                Complex h4 = Complex.ImaginaryOne * (-_B * Complex.Conjugate(_C) + _A * Complex.Conjugate(_D)) * Q2_distribution_k(data, i);

                xs[i] = i;
                ys[i] = (h1 + h2 + h3 + h4).Imaginary;
            });
            return (xs, ys);
        }
        public static double Q1_distribution_k(InitialData data, double P2)
        {
            Complex h1 = A(data) * Complex.Conjugate(C(data)) * data.VoltNom * data.VoltNom;
            Complex h2 = B(data) * Complex.Conjugate(D(data)) * (Math.Pow(P2, 2) + Math.Pow(Q2_distribution_k(data, P2), 2)) / data.VoltNom / data.VoltNom;
            Complex h3 = (B(data) * Complex.Conjugate(C(data)) + A(data) * Complex.Conjugate(D(data))) * P2;
            Complex h4 = Complex.ImaginaryOne * (-B(data) * Complex.Conjugate(C(data)) + A(data) * Complex.Conjugate(D(data))) * Q2_distribution_k(data, P2);
            double Q1 = (h1 + h2 + h3 + h4).Imaginary;
            return Q1;
        }

        #endregion

        public class RowShell : INotifyPropertyChanged
        {
            public string Name { get; set; }
            public Complex? Value1
            {
                get => _value1;
                set
                {
                    _value1 = value;
                    NotifyPropertyChanged("Value1");
                }
            }
            private Complex? _value1;

            public Complex? Value2
            {
                get => _value2;
                set
                {
                    _value2 = value;
                    NotifyPropertyChanged("Value2");
                }
            }
            private Complex? _value2;

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public override string ToString()
            {
                return $"{Name}: {Value1}, {Value2}";
            }
        }
    }
}
