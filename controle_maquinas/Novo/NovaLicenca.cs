﻿using Fd_DBC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controle_maquinas
{
    public partial class NovaLicenca : Form
    {
        DBC CG = new DBC();

        public NovaLicenca()
        {
            InitializeComponent();
        }

        //Metodos
        private void ConfigCBB()
        {
            cbbSoftware.Items.Clear();

            cbbSoftware.Items.Add("Selecionar Software");
            cbbSoftware.SelectedIndex = 0;

            //Pesquisam os nome dos Software no banco de dados
            string cmd = "SELECT * FROM software;";
            CG.ExecutarComandoSql(cmd);

            //Declara um DataTable
            DataTable dt;
            //Instancia o DataTable e insere o retorno do banco de dados no mesmo
            dt = new DataTable();
            CG.RetornarDadosDataTable(dt);

            string Software = "";

            //Para Cada Software inserido no DataTable
            foreach (DataRow r in dt.Rows)
            {
                //Retorna o nome do cliente na variável
                Software = r[1].ToString();

                //Insere o nome do cliente na ComboBox
                cbbSoftware.Items.Add(Software);
            }
        }

        //Form
        private void NovaLicenca_Load(object sender, EventArgs e)
        {
            rdbUnica.Checked = true;
            txtQuantidade.ReadOnly = true;
            txtQuantidade.Text = "1";
            
            ConfigCBB();
        }

        //Botao
        private void btnSoftware_Click(object sender, EventArgs e)
        {
            NovoSoftware form = new NovoSoftware();
            form.ShowDialog();
            ConfigCBB();
        }
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtKey.Clear();
            txtNfe.Clear();
        }
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (cbbSoftware.Text != "Selecionar Software")
            {
                if (txtKey.Text.Trim() != "")
                {
                    if (txtNfe.Text.Trim() != "")
                    {
                        string Software = cbbSoftware.Text;
                        string Key = txtKey.Text;
                        string nfe = txtNfe.Text;
                        string observacao = txtObservacao.Text;
                        string licenca = "";
                        int qtd = int.Parse(txtQuantidade.Text);
                        int qtdmax = int.Parse(txtQuantidade.Text);

                        string cmd = "INSERT INTO `software_licencas` " +
                            "(`software`, `key`, `qtd`, `qtdmax`, `nfe`, `observacao`) " +
                            "VALUES ('" + Software + "', '" + Key + "', '" + qtd + "', '" + qtdmax + "', '" + nfe + "', '" + observacao + "');";
                        CG.ExecutarComandoSql(cmd);

                        txtKey.Clear();
                        txtNfe.Clear();
                        txtObservacao.Clear();
                        txtQuantidade.Text = "1";
                        rdbUnica.Checked = true;
                        cbbSoftware.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show("Campo NF-e vazio");
                    }
                }
                else
                {
                    MessageBox.Show("Campo Key vazio");
                }
            }
            else
            {
                MessageBox.Show("Software Não Selecionado");
            }
        }

        private void rdbMultiplas_CheckedChanged(object sender, EventArgs e)
        {
            if(rdbMultiplas.Checked == true)
            {
                txtQuantidade.ReadOnly = false;
            }
        }
        private void rdbUnica_CheckedChanged(object sender, EventArgs e)
        {
            if(rdbUnica.Checked == true)
            {
                txtQuantidade.ReadOnly = true;
                txtQuantidade.Text = "1";
            }
        }

        //textBox
        private void txtQuantidade_TextChanged(object sender, EventArgs e)
        {
            bool ivia;

            ivia = CG.ValidarNumero(txtQuantidade.Text);

            if(ivia == false)
            {
                txtQuantidade.Clear();
            }
        }
    }
}