﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class FrmNuevosEmpleados : MetroFramework.Forms.MetroForm
    {
        private bool IsNuevo = true;
        private bool IsEditar = false;

        private static FrmNuevosEmpleados _Instancia;

        public static FrmNuevosEmpleados GetInstancia()
        {
            if (_Instancia == null)
            {
                _Instancia = new FrmNuevosEmpleados();
            }
            return _Instancia;
        }

        public void setCandidatos(string cedula, string nombre, string apellido, string idiomas, string competencias, string capacitaciones, string experiencia, string recomendado)
        {
            this.txtCedula.Text = cedula;
            this.txtNombre.Text = nombre;
            this.txtApellido.Text = apellido;
            //this.txtPuestos.Text = puesto;
            this.txtIdiomas.Text = idiomas;
            this.txtCompetencias.Text = competencias;
            this.txtCapacitaciones.Text = capacitaciones;
            this.txtExperiencia.Text = experiencia;
            this.txtRecomendado.Text = recomendado;
        }

        //Mostrar mensaje de confirmacion
        private void MensajeOk(string mensaje)
        {
            MessageBox.Show(mensaje, "Sistema RRHH", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Mostrar mensaje de error
        private void MensajeError(string mensaje)
        {
            MessageBox.Show(mensaje, "Sistema RRHH", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Limpiar todos los controles del formulario
        private void Limpiar()
        {
            this.txtCedula.Text = string.Empty;
            this.txtNombre.Text = string.Empty;
            this.txtApellido.Text = string.Empty;
            this.txtApellido.Text = string.Empty;
            this.txtSalario.Text = string.Empty;
            this.pbImagen.Image = global::CapaPresentacion.Properties.Resources.file;

        }

        public FrmNuevosEmpleados()
        {
            InitializeComponent();
            this.LlenarComboDepartamentoss();
            this.LlenarComboPuestos();

            //Inhabilitar textbox
            this.txtCedula.ReadOnly = true;
            this.txtNombre.ReadOnly = true;
            this.txtApellido.ReadOnly = true;
            this.txtRecomendado.ReadOnly = true;
            this.txtIdiomas.ReadOnly = true;
            this.txtCapacitaciones.ReadOnly = true;
            this.txtCompetencias.ReadOnly = true;
            //this.txtPuestos.ReadOnly = true;

        }
        private void LlenarComboPuestos()
        {
            cmbPuestos.DataSource = NPuestos.Mostrar();
            cmbPuestos.ValueMember = "id_puestos";
            cmbPuestos.DisplayMember = "Nombre";
        }

        private void LlenarComboDepartamentoss()
        {
            cmbDepartamentos.DataSource = NDepartamentos.Mostrar();
            cmbDepartamentos.ValueMember = "id_departamento";
            cmbDepartamentos.DisplayMember = "Nombre";
        }

        private void FrmNuevosEmpleados_Load(object sender, EventArgs e)
        {

        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pbImagen.SizeMode = PictureBoxSizeMode.StretchImage;
                this.pbImagen.Image = Image.FromFile(dialog.FileName);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.pbImagen.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pbImagen.Image = global::CapaPresentacion.Properties.Resources.file;
        }

        private void chkEstado_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEstado.Checked)
            {
                chkEstado.ThreeState = true;
                chkEstado.CheckAlign = ContentAlignment.MiddleRight;
                chkEstado.Text = "True";
            }
            else if (!chkEstado.Checked)
            {
                chkEstado.ThreeState = false;
                chkEstado.CheckAlign = ContentAlignment.MiddleLeft;
                chkEstado.Text = "False";
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string rpta = "";
                if (this.txtCedula.Text == string.Empty)
                {
                    MensajeError("Faltan ingresar algunos datos, seran remarcados");
                }
                else
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    this.pbImagen.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                    byte[] image = ms.GetBuffer();
                    if (this.IsNuevo)
                    {
                        rpta = NEmpleados.Insertar(this.txtCedula.Text.Trim(), this.txtNombre.Text.Trim().ToUpper(),
                            this.txtApellido.Text.Trim().ToUpper(), this.dtpFechaIngreso.Value.Date,
                            Convert.ToInt32(this.cmbPuestos.SelectedValue), Convert.ToInt32(this.cmbDepartamentos.SelectedValue),
                             image, Convert.ToDecimal(this.txtSalario.Text), this.chkEstado.Text.Trim());
                    }

                    if (rpta.Equals("OK"))
                    {
                        if (this.IsNuevo)
                        {
                            this.MensajeOk("Se inserto de forma correcta el registro");
                        }
                        else
                        {
                            this.MensajeOk("Se actualizo de forma correcta el registro");
                        }
                    }
                    else
                    {
                        this.MensajeError(rpta);
                    }

                    this.IsNuevo = false;
                    this.IsEditar = false;
                    //this.Botones();
                    this.Limpiar();
                    //this.Mostrar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
    }
}