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
    public partial class FrmExperencia : Form
    {
        private bool IsNuevo = false;
        private bool IsEditar = false;

        public FrmExperencia()
        {
            InitializeComponent();
            this.ttMensaje.SetToolTip(this.txtEmpresa, "Ingrese el nombre de la empresa a la cual  pertenecia");
            this.ttMensaje.SetToolTip(this.txtSalario, "Ingrese el salario bruto que ganaba");
            this.ttMensaje.SetToolTip(this.txtPuesto, "Ingrese cual era su antgua posicion");
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
            this.txtEmpresa.Text = string.Empty;
            this.txtPuesto.Text = string.Empty;
            this.txtIdExperencia.Text = string.Empty;
            this.txtSalario.Text = string.Empty;
        }

        //Habilitar los controles del formulario
        private void Habilitar(bool valor)
        {
            this.txtEmpresa.ReadOnly = !valor;
            this.txtIdExperencia.ReadOnly = !valor;
            this.txtPuesto.ReadOnly = !valor;
            this.txtSalario.ReadOnly = !valor;
        }

        //Habilitar los botones
        private void Botones()
        {
            if (this.IsNuevo || this.IsEditar)
            {
                this.Habilitar(true);
                this.btnNuevo.Enabled = false;
                this.btnGuardar.Enabled = true;
                this.btnEditar.Enabled = false;
                this.btnCancelar.Enabled = true;
            }
            else
            {
                this.Habilitar(false);
                this.btnNuevo.Enabled = true;
                this.btnGuardar.Enabled = false;
                this.btnEditar.Enabled = true;
                this.btnCancelar.Enabled = false;
            }
        }

        //Metodo para Ocultar columnas
        private void OcultarColumnas()
        {
            this.dataListado.Columns[0].Visible = false;
            this.dataListado.Columns[1].Visible = false;
        }

        //Metodo mostrar
        private void Mostrar()
        {
            this.dataListado.DataSource = NExperencia.Mostrar();
            this.OcultarColumnas();
            lblTotal.Text = "Total de registro: " + Convert.ToString(dataListado.Rows.Count);
        }

        //Metodo BuscarNombre
        private void BuscarNombre()
        {
            this.dataListado.DataSource = NExperencia.BuscarNombre(this.txtBuscar.Text);
            this.OcultarColumnas();
            lblTotal.Text = "Total de registro: " + Convert.ToString(dataListado.Rows.Count);
        }

        private void FrmExperencia_Load(object sender, EventArgs e)
        {
            this.Top = 0;
            this.Left = 0;

            this.Mostrar();
            this.Habilitar(false);
            this.Botones();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            this.BuscarNombre();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            this.BuscarNombre();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            this.IsNuevo = true;
            this.IsEditar = false;
            this.Botones();
            this.Limpiar();
            this.Habilitar(true);
            this.txtEmpresa.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string rpta = "";
                if (this.txtEmpresa.Text == string.Empty)
                {
                    MensajeError("Faltan ingresar algunos datos, seran remarcados");
                    erroricono.SetError(txtEmpresa, "Ingrese un nombre");
                }
                else
                {
                    if (this.IsNuevo)
                    {
                        rpta = NExperencia.Insertar(this.txtEmpresa.Text.Trim().ToUpper(), this.txtPuesto.Text.Trim().ToUpper(), this.dtpFechaInicio.Value.Date, this.dtpFechaFin.Value.Date, Convert.ToDecimal(this.txtSalario.Text));
                    }
                    else
                    {
                        rpta = NExperencia.Editar(Convert.ToInt32(this.txtIdExperencia.Text), this.txtEmpresa.Text.Trim().ToUpper(), this.txtPuesto.Text.Trim().ToUpper(), this.dtpFechaInicio.Value.Date, this.dtpFechaFin.Value.Date, Convert.ToDecimal(this.txtSalario.Text));
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
                    this.Botones();
                    this.Limpiar();
                    this.Mostrar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (!this.txtIdExperencia.Text.Equals(""))
            {
                this.IsEditar = true;
                this.Botones();
                this.Habilitar(true);
            }
            else
            {
                this.MensajeError("Debe de seleccionar primero el registro a modificar");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.IsNuevo = false;
            this.IsEditar = false;
            this.Botones();
            this.Limpiar();
            this.Habilitar(false);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult Opcion;
                Opcion = MessageBox.Show("Realmente quieres eliminar los registros", "Sistema RRHH", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (Opcion == DialogResult.OK)
                {
                    string Codigo;
                    string Rpta = "";

                    foreach (DataGridViewRow row in dataListado.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            Codigo = Convert.ToString(row.Cells[1].Value);
                            Rpta = NExperencia.Eliminar(Convert.ToInt32(Codigo));

                            if (Rpta.Equals("OK"))
                            {
                                this.MensajeOk("Se elimino correctamente el registro");
                            }
                            else
                            {
                                this.MensajeError(Rpta);
                            }
                        }
                    }
                    this.Mostrar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
    }
}