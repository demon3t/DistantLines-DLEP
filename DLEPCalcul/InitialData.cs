using System;
using System.Collections.Generic;
using System.Text;

namespace DLEPCalcul
{
    public class InitialData
    {
        public event Action ValueChange;

        public InitialData()
        {
            VoltNom = 500;
            Length = 700;
            N_split = 3;
            F_st = 500;
            F_al = 336;
            D_wire = 37.5;
            R0 = 0.02;
            D_phase = 11.7;
            A_splitwires = 48;
            K1 = 0.55;
            K2 = 1.45;
        }

        public InitialData(InitialData data, int value)
        {
            VoltNom = data.VoltNom;
            Length = data.Length;
            N_split = data.N_split;
            F_st = data.N_split;
            F_al = data.F_al;
            D_wire = data.D_wire;
            R0 = value;
            D_phase = data.D_phase;
            A_splitwires = data.A_splitwires;
            K1 = data.K1;
            K2 = data.K2;
        }

        #region Свойства

        /// <summary>
        /// Номинальное напряжение
        /// </summary>
        public double VoltNom
        {
            get { return _voltNom; }
            set
            {
                _voltNom = value;
                ValueChange?.Invoke();
            }
        }
        private double _voltNom;

        /// <summary>
        /// Длина динии
        /// </summary>
        public double Length
        {
            get { return _length; }
            set
            {
                _length = value;
                ValueChange?.Invoke();
            }
        }
        private double _length;

        /// <summary>
        /// Кол-во расщеплённых проводов
        /// </summary>
        public int N_split
        {
            get { return _n_split; }
            set
            {
                _n_split = value;
                ValueChange?.Invoke();
            }
        }
        private int _n_split;

        /// <summary>
        /// Активное сопротивление
        /// </summary>
        public double R0
        {
            get { return _r0; }
            set
            {
                _r0 = value;
                ValueChange?.Invoke();
            }
        }
        private double _r0;

        /// <summary>
        /// Расстояние между фазами
        /// </summary>
        public double D_phase
        {
            get { return _d_phase; }
            set
            {
                _d_phase = value;
                ValueChange?.Invoke();
            }
        }
        private double _d_phase;

        /// <summary>
        /// Расстояние между расщеплёнными проводами
        /// </summary>
        public double A_splitwires
        {
            get { return _a_splitwire; }
            set
            {
                _a_splitwire = value;
                ValueChange?.Invoke();
            }
        }
        private double _a_splitwire;

        /// <summary>
        /// Диаметр провода
        /// </summary>
        public double D_wire
        {
            get { return _d_wire; }
            set
            {
                _d_wire = value;
                ValueChange?.Invoke();
            }
        }
        private double _d_wire;

        /// <summary>
        /// Сечение стали
        /// </summary>
        public double F_st
        {
            get { return _f_st; }
            set
            {
                _f_st = value;
                ValueChange?.Invoke();
            }
        }
        private double _f_st;

        /// <summary>
        /// Сечение алюминия
        /// </summary>
        public double F_al
        {
            get { return _f_al; }
            set
            {
                _f_al = value;
                ValueChange?.Invoke();
            }
        }
        private double _f_al;

        public double K1
        {
            get { return _k1; }
            set
            {
                _k1 = value;
                ValueChange?.Invoke();
            }
        }
        private double _k1;

        public double K2
        {
            get { return _k2; }
            set
            {
                _k2 = value;
                ValueChange?.Invoke();
            }
        }
        private double _k2;

        #endregion
    }
}

