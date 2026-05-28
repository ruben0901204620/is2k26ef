using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Capa_Vista_Seguridad;
using Capa_Controlador_Seguridad;
using System.Drawing.Imaging;


namespace Capa_Vista_Logista
{
    public partial class Frm_MDI : Form
    {
        private Cls_ControladorAsignacionUsuarioAplicacion controladorPermisos = new Cls_ControladorAsignacionUsuarioAplicacion();
        private Cls_Asignacion_Permiso_PerfilControlador controladorPermisosPerfil = new Cls_Asignacion_Permiso_PerfilControlador();

        public enum MenuOpciones
        {
            Archivo,
            Catalogos,
            Procesos,
            Reportes,
            Ayudas,
            Seguridad
        }

        private Dictionary<MenuOpciones, ToolStripMenuItem> menuItems;

        public Frm_MDI()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            InicializarMenuItems();
            fun_inicializar_botones_por_defecto();

            this.Load += Frm_MDI_Load;
            this.IsMdiContainer = true;

        }

        private void Frm_MDI_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = $"Estado: Conectado | Usuario: {Capa_Controlador_Seguridad.Cls_Usuario_Conectado.sNombreUsuario}";
            fun_inicializar_botones_por_defecto();
            fun_habilitar_botones_por_permisos_combinados(
                Cls_Usuario_Conectado.iIdUsuario,
                Cls_Usuario_Conectado.iIdPerfil
            );
        }

        private void InicializarMenuItems()
        {
            menuItems = new Dictionary<MenuOpciones, ToolStripMenuItem>
            {
                { MenuOpciones.Archivo, archivoToolStripMenuItem },
                //{ MenuOpciones.Catalogos, catálogossToolStripMenuItem },
                { MenuOpciones.Procesos, procesosToolStripMenuItem },
                { MenuOpciones.Reportes, herramientasToolStripMenuItem },
                { MenuOpciones.Ayudas, asignacionesToolStripMenuItem },
                { MenuOpciones.Seguridad, seguridadToolStripMenuItem }
            };
        }

        public void fun_inicializar_botones_por_defecto()
        {
            foreach (var opcion in menuItems.Keys)
            {
                switch (opcion)
                {
                    case MenuOpciones.Archivo:
                    case MenuOpciones.Reportes:
                    case MenuOpciones.Ayudas:
                    case MenuOpciones.Catalogos:
                    case MenuOpciones.Procesos:
                        menuItems[opcion].Enabled = true;
                        break;
                    default:
                        menuItems[opcion].Enabled = true;
                        break;
                }
            }
        }

        public void fun_habilitar_botones_por_permisos_combinados(int iIdUsuario, int iIdPerfil)
        {
            // CATÁLOGOS: 700-709
            Dictionary<int, ToolStripMenuItem> mapaCatalogos = new Dictionary<int, ToolStripMenuItem>
            {
                //{708, cuentasPorPagarToolStripMenuItem}
                
            };

            // PROCESOS: 710-734 (agregar cuando estén listos)
            Dictionary<int, ToolStripMenuItem> mapaProcesos = new Dictionary<int, ToolStripMenuItem>
            {
                
                //{730, consultaDeInventariosToolStripMenuItem_Click},
            };
            Dictionary<int, ToolStripMenuItem> mapaReportes = new Dictionary<int, ToolStripMenuItem>
            {
                //{733, balanceDeAntiguedadToolStripMenuItem },

            };


            foreach (var sub in mapaCatalogos.Values) sub.Enabled = true;
            foreach (var sub in mapaProcesos.Values) sub.Enabled = true;
            foreach (var sub in mapaReportes.Values) sub.Enabled = true;
            menuItems[MenuOpciones.Seguridad].Enabled = true;

            DataTable dtPermisosPerfil = controladorPermisosPerfil.datObtenerPermisosPorPerfil(iIdPerfil);
            foreach (DataRow row in dtPermisosPerfil.Rows)
            {
                int idModulo = Convert.ToInt32(row["iFk_id_modulo"]);
                int idAplicacion = Convert.ToInt32(row["iFk_id_aplicacion"]);

                if (idModulo == 44 && idAplicacion >= 700 && idAplicacion <= 734)
                {
                    if (mapaCatalogos.ContainsKey(idAplicacion))
                        mapaCatalogos[idAplicacion].Enabled = true;
                    if (mapaProcesos.ContainsKey(idAplicacion))
                        mapaProcesos[idAplicacion].Enabled = true;
                    if (mapaReportes.ContainsKey(idAplicacion))
                        mapaReportes[idAplicacion].Enabled = true;
                }

                if (idModulo == 4 && idAplicacion == 309)
                {
                    menuItems[MenuOpciones.Seguridad].Enabled = true;
                }
            }

            DataTable dtPermisosUsuario = controladorPermisos.ObtenerPermisosPorUsuario(iIdUsuario);
            foreach (DataRow row in dtPermisosUsuario.Rows)
            {
                int idModulo = Convert.ToInt32(row["iFk_id_modulo"]);
                int idAplicacion = Convert.ToInt32(row["iFk_id_aplicacion"]);

                if (idModulo == 44 && idAplicacion >= 700 && idAplicacion <= 734)
                {
                    if (mapaCatalogos.ContainsKey(idAplicacion))
                        mapaCatalogos[idAplicacion].Enabled = true;
                    if (mapaProcesos.ContainsKey(idAplicacion))
                        mapaProcesos[idAplicacion].Enabled = true;
                    if (mapaReportes.ContainsKey(idAplicacion))
                        mapaReportes[idAplicacion].Enabled = true;
                }

                if (idModulo == 4 && idAplicacion == 309)
                {
                    menuItems[MenuOpciones.Seguridad].Enabled = true;
                }
            }

            //menuItems[MenuOpciones.Catalogos].Enabled = mapaCatalogos.Values.Any(m => m.Enabled);
            menuItems[MenuOpciones.Procesos].Enabled = mapaProcesos.Values.Any(m => m.Enabled);
            menuItems[MenuOpciones.Reportes].Enabled = mapaProcesos.Values.Any(m => m.Enabled);
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Frm_LOGIN login = new Frm_LOGIN();
            login.ShowDialog();
            this.Close();
        }


        private void consultaDeBitácoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Capa_Vista_Seguridad.Frm_Bitacora bitacoras = new Capa_Vista_Seguridad.Frm_Bitacora();
            bitacoras.StartPosition = FormStartPosition.CenterScreen;
            bitacoras.Show();

        }

        private void cambiarContraseñaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_cambiar_contrasena contrasena = new Frm_cambiar_contrasena(Capa_Controlador_Seguridad.Cls_Usuario_Conectado.iIdUsuario);
            contrasena.StartPosition = FormStartPosition.CenterScreen;
            contrasena.Show();

        }

        private void mantenimientoAplicacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_asignacion_aplicacion_usuario app = new Frm_asignacion_aplicacion_usuario();
            app.StartPosition = FormStartPosition.CenterScreen;
            app.Show();
        }

        private void crearUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Usuario usr = new Frm_Usuario();
            usr.StartPosition = FormStartPosition.CenterScreen;
            usr.Show();
        }



        private void seccionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VIsta_seccion.frm_seccion secciones = new VIsta_seccion.frm_seccion();
            secciones.StartPosition = FormStartPosition.CenterScreen;
            secciones.Show();
        }
    }
}