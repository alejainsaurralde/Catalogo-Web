﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Service;

namespace Catalogo_Web
{
    public partial class MiPerfil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Seguridad.sesionActiva(Session["usuario"]))
                    {
                        Usuario user = (Usuario)Session["usuario"];
                        txtEmail.Text = user.Email;
                        txtEmail.ReadOnly = true;
                        txtNombre.Text = user.Nombre;
                        txtApellido.Text = user.Apellido;
                        txtFechaNacimiento.Text = user.FechaNacimiento.ToString("yyyy-MM-dd");
                        if (!string.IsNullOrEmpty(user.ImagenPerfil))
                            imgNuevoPerfil.ImageUrl = "~/Images/" + user.ImagenPerfil;
                    }
                }

            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
            }

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
      
            try
            {
                UsuarioService service = new UsuarioService();
                Usuario user = (Usuario)Session["usuario"];
                //Escribir img si se cargó algo.
                if (txtImagen.PostedFile.FileName != "")
                {
                    string ruta = Server.MapPath("./Images/");
                    txtImagen.PostedFile.SaveAs(ruta + "perfil-" + user.Id + ".jpg");
                    user.ImagenPerfil = "perfil-" + user.Id + ".jpg";
                }

                user.Nombre = txtNombre.Text;
                user.Apellido = txtApellido.Text;
                user.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);

                //guardar datos perfil
                service.actualizar(user);

                //leer img
                Image img = (Image)Master.FindControl("imgAvatar");
                img.ImageUrl = "~/Images/" + user.ImagenPerfil;

            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
            }
        }
    }
}