﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO.Ports;

namespace TrucoClient
{
    public partial class TrucoForm : Form
    {
        public TrucoForm()
        {
            InitializeComponent();
        }
        
        //-----Se inicializan los buffers y se desabilitan los botones 'CONECTAR' y 'ENVIAR DATOS'
        private void TrucoForm_Load(object sender, EventArgs e)
        {
            strBufferIn = "";
            strBufferOut = "";
            BtnConectar.Enabled = false;
            BtnEnviarDatos.Enabled = false;
            BtnNext.Enabled = false;
        }

        //------------DELEGADO ???-------------------
        private delegate void DelegadoAcceso(string accion);
        //------------Strings donde se enviarán y recibirán los datos----------
        private string strBufferIn;
        private string strBufferOut;
        public string computadora;
        public string carta;

        TrucoGame gameForm = new TrucoGame();

        private void AccesoForm(string accion)
        {
            strBufferIn = accion;
            //---------AQUÍ SE MANEJA EL MENSAJE RECIBIDO-------------
            TxtDatosRecibidos.Text = strBufferIn;
            gameForm.TxtLastData.Text = strBufferIn;

            if(strBufferIn[0] == 'I')
            {
                //SE ASUME QUE SE VA A REPARTIR
                if (strBufferIn[1] == computadora[0])
                {
                    //SI LAS CARTAS SON PARA MI SE MUESTRAN LAS CARTAS
                    for (int i = 2; i < 8; i++)
                    {
                        if (i == 2)
                        {
                            carta = "";
                            carta = carta + strBufferIn[i];
                        }

                        if (i == 3)
                        {
                            carta = carta + strBufferIn[i];
                            gameForm.PbCarta1.Image = Image.FromFile(carta + ".png");
                            gameForm.PbCarta1.Tag = carta;
                        }

                        if (i == 4)
                        {
                            carta = "";
                            carta = carta + strBufferIn[i];
                        }

                        if (i == 5)
                        {
                            carta = carta + strBufferIn[i];
                            gameForm.PbCarta2.Image = Image.FromFile(carta + ".png");
                            gameForm.PbCarta2.Tag = carta;
                                                    }

                        if (i == 6)
                        {
                            carta = "";
                            carta = carta + strBufferIn[i];
                        }

                        if (i == 7)
                        {
                            carta = carta + strBufferIn[i];
                            gameForm.PbCarta3.Image = Image.FromFile(carta + ".png");
                            gameForm.PbCarta3.Tag = carta;

                        }
                    }
                }
                else if(strBufferIn[1] == 'T')
                {
                    carta = strBufferIn[3].ToString() + strBufferIn[4].ToString();
                    gameForm.PbVira.Image = Image.FromFile(carta + ".png");
                }
                else
                {
                    //SI LAS CARTAS NO SON PARA MI SE REENVIA LA TRAMA
                    EnviarDatos(strBufferIn);
                }


            }else if (strBufferIn[0] == 'T')
            {
                //SE MUESTRA EL BOMBILLITO DEL TURNO EN LA PANTALLA                
                if (strBufferIn[1] == 'A')
                {
                    gameForm.PbTurnoA.Image = Image.FromFile("turno.png");

                    gameForm.PbTurnoA.Visible = false;
                    gameForm.PbTurnoB.Visible = false;
                    gameForm.PbTurnoC.Visible = false;
                    gameForm.PbTurnoD.Visible = false;


                    gameForm.PbTurnoA.Visible = true;
                }
                if (strBufferIn[1] == 'B')
                {
                    gameForm.PbTurnoB.Image = Image.FromFile("turno.png");

                    gameForm.PbTurnoA.Visible = false;
                    gameForm.PbTurnoB.Visible = false;
                    gameForm.PbTurnoC.Visible = false;
                    gameForm.PbTurnoD.Visible = false;

                    gameForm.PbTurnoB.Visible = true;
                }
                if (strBufferIn[1] == 'C')
                {
                    gameForm.PbTurnoC.Image = Image.FromFile("turno.png");

                    gameForm.PbTurnoA.Visible = false;
                    gameForm.PbTurnoB.Visible = false;
                    gameForm.PbTurnoC.Visible = false;
                    gameForm.PbTurnoD.Visible = false;

                    gameForm.PbTurnoC.Visible = true;
                }
                if (strBufferIn[1] == 'D')
                {
                    gameForm.PbTurnoD.Image = Image.FromFile("turno.png");

                    gameForm.PbTurnoA.Visible = false;
                    gameForm.PbTurnoB.Visible = false;
                    gameForm.PbTurnoC.Visible = false;
                    gameForm.PbTurnoD.Visible = false;

                    gameForm.PbTurnoD.Visible = true;                   
                }

                //SI ES TU TURNO JUEGAS 

                if(computadora[0] == strBufferIn[1])
                {
                    //SE HABILITAN LAS CARTAS
                    gameForm.PbCarta1.Enabled = true;
                    gameForm.PbCarta2.Enabled = true;
                    gameForm.PbCarta3.Enabled = true;
                }

            }

            //--------SIMULACIÓN DE PEDIR ENVIDO---------------
            /* if (strBufferIn[7] == 'S')
             {
                 EnviarDatos("$$STSG#S######%%");
             }*/
            //--------------------------------------------------------
        }

        public void EnvioForm()
        {
            if(gameForm.PbCartaA.Enabled == true)
            {

            }

            if (gameForm.PbCartaB.Enabled == true)
            {

            }

            if (gameForm.PbCartaC.Enabled == true)
            {

            }

            if (gameForm.PbCartaD.Enabled == true)
            {

            }
        }

        //------------DELEGADO ???-------------------
        private void AccesoInterrupcion(string accion)
        {
            DelegadoAcceso Var_DelegadoAcceso;
            Var_DelegadoAcceso = new DelegadoAcceso(AccesoForm);
            object[] arg = { accion };
            base.Invoke(Var_DelegadoAcceso, arg);
        }

        private void DatoRecibido(object sender, SerialDataReceivedEventArgs e)
        {
            //----Se recibe el mensaje a través de DELEGADOS ???--------------
            AccesoInterrupcion(SpPuertos.ReadExisting());

            /*string Data_in = SpPuertos.ReadExisting();
            MessageBox.Show(Data_in);
            TxtDatosRecibidos.Text = Data_in;*/
        }

        private void BtnBuscarPuertos_Click(object sender, EventArgs e)
        {
            //Se buscan los puertos disponibles de la computadora
            string[] PuertosDisponibles = SerialPort.GetPortNames();

            //Se limpia el comboBox
            CboPuertos.Items.Clear();

            //Se llena el comboBox con los puertos disponibles
            foreach (string puerto in PuertosDisponibles)
            {
                CboPuertos.Items.Add(puerto);
            }

            //Si hay puertos disponibles se habilita 'CONECTAR' si no se avisa que no hay puertos disponibles
            if (CboPuertos.Items.Count > 0)
            {
                CboPuertos.SelectedIndex = 0;
                MessageBox.Show("SELECCIONA UN PUERTO");
                BtnConectar.Enabled = true;
            }
            else
            {
                MessageBox.Show("NO SE DETECTARON PUERTOS DISPONIBLES");
                CboPuertos.Items.Clear();
                CboPuertos.Text = "                     ";
                strBufferIn = "";
                strBufferOut = "";
                BtnConectar.Enabled = false;
                BtnEnviarDatos.Enabled = false;
            }
        }

        private void BtnConectar_Click(object sender, EventArgs e)
        {
            try
            {
                //'CONECTAR' configura el puerto, lo abre, cambia a 'DESCONECTAR' y habilita el botón 'Enviar Datos'
                if (BtnConectar.Text == "CONECTAR")
                {
                    SpPuertos.BaudRate = Int32.Parse(CboBaudRate.Text);
                    SpPuertos.DataBits = 8;
                    SpPuertos.Parity = Parity.None;
                    SpPuertos.StopBits = StopBits.One;
                    SpPuertos.Handshake = Handshake.None;
                    SpPuertos.PortName = CboPuertos.Text;

                   // gameForm.SpPuertosGame = SpPuertos;

                    try
                    {
                        SpPuertos.Open();
                       // gameForm.SpPuertosGame.Open();

                        BtnConectar.Text = "DESCONECTAR";
                        BtnEnviarDatos.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                //'DESCONECTAR' cierra el puerto, cambia a 'CONECTAR' y desabilita el botón 'Enviar Datos'
                else if (BtnConectar.Text == "DESCONECTAR")
                {
                    SpPuertos.Close();
                    BtnConectar.Text = "CONECTAR";
                    BtnEnviarDatos.Enabled = false;
                }

                BtnNext.Enabled = true;
            }
            catch (Exception ex)
            {
                //Si hay algún problema conectándose se muestra por pantalla el error
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void BtnEnviarDatos_Click(object sender, EventArgs e)
        {
            //Se llena el buffer de salida y se envían los datos
            try
            {
                //---------AQUÍ SE MANEJA EL MENSAJE ENVIADO-------------
                SpPuertos.DiscardOutBuffer();
                strBufferOut = TxtDatosEnviados.Text;
                SpPuertos.Write(strBufferOut);
                //-------------------------------------------------------
            }
            catch (Exception ex)
            {
                //Si hay algún problema enviando datos se muestra por pantalla el error
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void EnviarDatos(string msg)
        {
            //Se llena el buffer de salida y se envían los datos
            try
            {
                //---------AQUÍ SE MANEJA EL MENSAJE ENVIADO-------------
                SpPuertos.DiscardOutBuffer();
                strBufferOut = msg;
                SpPuertos.Write(msg);
                //-------------------------------------------------------
            }
            catch (Exception ex)
            {
                //Si hay algún problema enviando datos se muestra por pantalla el error
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {

            //SE OCULTA LA CONFIGURACIÓN Y SE MUESTRA EL JUEGO


            //SE OCULTA TODO
            LblTitulo.Visible = false;
            LblBaudRate.Visible = false;
            LblDatosRecibidos.Visible = false;
            BtnBuscarPuertos.Visible = false;
            BtnConectar.Visible = false;
            BtnEnviarDatos.Visible = false;
            BtnNext.Visible = false;
            CboPuertos.Visible = false;
            CboBaudRate.Visible = false;
            TxtDatosEnviados.Visible = false;
            TxtDatosRecibidos.Visible = false;


            //
            computadora = CboPlayer.Text;
            //

            //SE MUESTRA TODO
            gameForm.Show();
            gameForm.LblPlayer.Text = computadora;
            //this.Close();
        }

        private void LblSelectPlayer_Click(object sender, EventArgs e)
        {

        }
    }
}
