using Ej1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace Ej1.ViewModels
{
    public enum Ventanas { Agregar, Eliminar, Lista, Editar}
    public class LibrosViewModel : INotifyPropertyChanged
    {
        //coleccion de elementos 
        public ObservableCollection<Libro> Libros { get; set; } = new();
        //Acciones que realiza un modelo
        public ICommand AgregarCommand { get; set; }
        public ICommand VerAgregarCommand { get; set; }
        public ICommand VerEliminarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand VerEditarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public Ventanas Ventana { get; set; } = Ventanas.Lista; //para decidir que ventana mostrar
        //referencia a un modelo de libro
        public Libro? Libro { get; set; }
        public string Error { get; set; } //para mostrar errores en las vistas en vez de excepciones

        public LibrosViewModel()
        {
            CancelarCommand = new RelayCommand(Cancelar);
            AgregarCommand = new RelayCommand(Agregar);
            GuardarCommand = new RelayCommand(Guardar);
            VerAgregarCommand = new RelayCommand(VerAgregar);
            VerEditarCommand = new RelayCommand(VerEditar);
            VerEliminarCommand = new RelayCommand(VerEliminar);
            EliminarCommand = new RelayCommand(Eliminar);
            Deserializar();
        }

        private void Eliminar()
        {
            if(Libro != null)
            {
                Libros.Remove(Libro);
                Serializar();
                Ventana = Ventanas.Lista;
                Actualizar(nameof(Ventana));
            }
          
        }

        private void VerEliminar()
        {
            Ventana = Ventanas.Eliminar;
            Actualizar(nameof(Ventana));
        }

        int posAnterior;
        private void VerEditar()
        {
            if(Libro != null)
            {
                var clon = new Libro
                {
                    Autor = Libro.Autor,
                    Titulo = Libro.Titulo,
                    Portada = Libro.Portada,
                };

                posAnterior = Libros.IndexOf(Libro);
                Libro = clon;

                Ventana = Ventanas.Editar;
                Actualizar(nameof(Ventana));
            }
        }

        private void VerAgregar()
        {
            Libro = new(); //se captura un libro nuevo
            Error = "";
            Ventana = Ventanas.Agregar;
            Actualizar(nameof(Error));
            Actualizar(nameof(Ventana));
        }

        private void Guardar()
        {
            if (Libro != null)
            {
                Error = "";
                if (string.IsNullOrWhiteSpace(Libro.Titulo))
                {
                    Error = "Escriba el titulo del libro";
                    return;
                }
                if (string.IsNullOrWhiteSpace(Libro.Autor))
                {
                    Error = "Escriba el autor del libro";
                    return;
                }
                if (string.IsNullOrWhiteSpace(Libro.Portada) || !Libro.Portada.StartsWith("http")
                    || !Libro.Portada.EndsWith(".jpg"))
                {
                    Error = "Escriba la direccion de una imagen en JPEG";
                    return;
                }
                Actualizar(nameof(Error));

                if (Error == "")
                {
                    Libros[posAnterior]= Libro;
                    Serializar();
                    Ventana = Ventanas.Lista;
                    Actualizar(nameof(Ventana));
                  
                }
            }
        }

        private void Agregar()
        {
            Error = "";
            
            if (Libro != null)
            {
                
                if (string.IsNullOrWhiteSpace(Libro.Titulo))
                {
                    Error = "Escriba el titulo del libro";
                    return;
                }
                if (string.IsNullOrWhiteSpace(Libro.Autor))
                {
                    Error = "Escriba el autor del libro";
                    return;
                }
                if (string.IsNullOrWhiteSpace(Libro.Portada) || !Libro.Portada.StartsWith("http") 
                    || !Libro.Portada.EndsWith(".jpg"))
                {
                    Error = "Escriba la direccion de una imagen en JPEG";
                    return;
                }

                if (!string.IsNullOrWhiteSpace(Error))
                {
                    Actualizar(nameof(Error));
                    return;
                }
                Libros.Add(Libro);
                Serializar();
                Ventana = Ventanas.Lista;
                Actualizar(nameof(Ventana));

            }

        }

        private void Cancelar()
        {
            Ventana = Ventanas.Lista;
            Actualizar(nameof(Ventana));
        }

        private void Actualizar(string? name)
        {
            PropertyChanged?.Invoke(this, new(name));
        }

        public void Serializar()
        {
            File.WriteAllText("libros.json", JsonSerializer.Serialize(Libros));
        }

        public void Deserializar()
        {
            if (File.Exists("libros.json"))
            {
                var datos = JsonSerializer.Deserialize<ObservableCollection<Libro>>(File.ReadAllText("libros.json"));
                if (datos != null)
                {
                    foreach (var libro in datos)
                    {
                        Libros.Add(libro);
                    }
                }
            }
           
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
