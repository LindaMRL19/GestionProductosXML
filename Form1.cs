using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace GestionProductosXML
{
    public partial class Form1 : Form
    {
        private List<Producto> productos = new List<Producto>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Validar que los campos no estén vacíos
            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtPrecio.Text) || string.IsNullOrEmpty(txtCantidad.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }

            // Crear un nuevo producto con los valores ingresados
            Producto nuevoProducto = new Producto
            {
                Nombre = txtNombre.Text,
                Precio = Convert.ToDecimal(txtPrecio.Text),
                Cantidad = Convert.ToInt32(txtCantidad.Text)
            };

            // Agregar el producto a la lista
            productos.Add(nuevoProducto);

            // Mostrar el producto en el ListBox
            lstProducto.Items.Add($"{nuevoProducto.Nombre} - ${nuevoProducto.Precio} - {nuevoProducto.Cantidad} unidades");

            // Limpiar los campos para el siguiente ingreso
            txtNombre.Clear();
            txtPrecio.Clear();
            txtCantidad.Clear();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Ruta donde se guardará el archivo XML
                string ruta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\productos.xml";

                // Serializar la lista de productos a XML
                XmlSerializer serializer = new XmlSerializer(typeof(List<Producto>));
                using (StreamWriter writer = new StreamWriter(ruta))
                {
                    serializer.Serialize(writer, productos);
                }

                MessageBox.Show("Productos guardados en XML correctamente.", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar en XML: " + ex.Message, "Error");
            }
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                // Ruta del archivo XML
                string ruta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\productos.xml";

                // Verificar si el archivo existe
                if (!File.Exists(ruta))
                {
                    MessageBox.Show("No se encontró el archivo XML.", "Advertencia");
                    return;
                }

                // Deserializar el archivo XML a una lista de productos
                XmlSerializer serializer = new XmlSerializer(typeof(List<Producto>));
                using (StreamReader reader = new StreamReader(ruta))
                {
                    productos = (List<Producto>)serializer.Deserialize(reader);
                }

                // Actualizar el ListBox
                lstProducto.Items.Clear();
                foreach (var producto in productos)
                {
                    lstProducto.Items.Add($"{producto.Nombre} - ${producto.Precio} - {producto.Cantidad} unidades");
                }

                MessageBox.Show("Productos cargados desde XML correctamente.", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar desde XML: " + ex.Message, "Error");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show("¿Seguro que deseas salir?", "Confirmar salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
