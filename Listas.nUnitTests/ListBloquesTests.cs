﻿using Listas;
using Listas.Bloques;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;

namespace Listas.nUnitTests {
	[TestFixture]
	public class ListBloquesTests {

		private static ArrayBloque<int> _bloqueVacio,
			_bloqueConMenosElementos,
			_bloqueConElementos;

		private ArrayBloque<int>[] _bloques;

		[SetUp]
		public void SetUp() {
			_bloqueConElementos = new int[10] { 1, 2, 3, 4, 0, 0, 0, 0, 0, 0 };
			_bloqueConMenosElementos = new int[5] { 1, 2, 3, 4, 5 };
			_bloqueVacio = new int[10];
			_bloqueVacio.BorrarTodos();
			_bloques = [_bloqueVacio, _bloqueConMenosElementos, _bloqueConElementos];
		}

		[Test]
		public void Borrar_Bloque_EnLaLista_BorraElBloque() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();

			// Act
			listBloques.Insertar(_bloqueConElementos,0);
			listBloques.Insertar(_bloqueVacio, 1);

			var result = listBloques.Borrar(
				_bloqueConElementos);

			// Assert
			Assert.That(result, Is.EqualTo(0));
		}

		[Test]
		public void Borrar_Bloque_NoEnLaLista_NoBorraYDevuelveM1() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();
			int longitud;

			// Act
			listBloques.Insertar(_bloqueConElementos, 0);
			listBloques.Insertar(_bloqueVacio, 1);
			longitud = listBloques.Longitud;

			var result = listBloques.Borrar(
				_bloqueConMenosElementos);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result, Is.EqualTo(-1));
				Assert.That(longitud, Is.EqualTo(listBloques.Longitud));
			});
		}

		
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		public void BorrarBloque_CasosDePrueba(int value) {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int cantidad;
			for (int i = 0; i < _bloques.Length; i++) {
				listBloques.Insertar(_bloques[i], i);
			}
			cantidad = listBloques.CantidadBloques;

			// Act
			listBloques.BorrarBloque(value);
			
			// Assert
			Assert.That(cantidad, Is.EqualTo(listBloques.CantidadBloques + 1));
		}

		[TestCase(-1)]
		[TestCase(10)]
		public void BorrarBloque_ValoresInvalidos_LanzaExcepcion(int value) {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();
			int cantidad;
			for (int i = 0; i < _bloques.Length; i++) {
				listBloques.Insertar(_bloques[i], i);
			}

			cantidad = listBloques.CantidadBloques;

			// Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => listBloques.BorrarBloque(value));
		}

		[Test]
		public void BorrarUltimo_ConElementos_DevuelveElElementoYLoBorraYPuedeBorrarElUltimoBloque() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			var bloque = _bloqueConMenosElementos;
			listBloques.Insertar(_bloqueConMenosElementos, 0);
			var lleno = bloque.Lleno;
			var cantidad = listBloques.CantidadBloques;
			var ultimo = bloque.UltimoElemento;
			var longitudInicial = bloque.Longitud;

			// Act
			var result = listBloques.BorrarUltimo();

			// Assert
			Assert.Multiple(() => {
				Assert.That(lleno ^ cantidad == listBloques.CantidadBloques);
				Assert.That(ultimo, Is.EqualTo(result));
				Assert.That(listBloques.Longitud, Is.EqualTo(longitudInicial - 1));
			});
		}

		[Test]
		public void BorrarUltimo_SinElementos_LanzaUnaExcepcion() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();

			// Assert
			Assert.Throws<InvalidOperationException>(() => listBloques.BorrarUltimo());
		}

		[Test]
		public void BorrarPrimero_ConElementos_DosBloques_DevuelveElElementoYLoBorraYPuedeBorrarElUltimoBloque() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();
			var bloque = _bloqueConMenosElementos;
			listBloques.Insertar(_bloqueConMenosElementos, 0);
			var lleno = bloque.Lleno;
			var cantidad = listBloques.CantidadBloques;
			var primero = bloque.PrimerElemento;
			var longitudInicial = listBloques.Longitud;

			// Act
			var result = listBloques.BorrarPrimero();

			// Assert
			Assert.Multiple(() => {
				Assert.That(lleno ^ cantidad == listBloques.CantidadBloques);
				Assert.That(primero, Is.EqualTo(result));
				Assert.That(listBloques.Longitud, Is.EqualTo(longitudInicial - 1));
			});
		}

		[Test]
		public void BorrarPrimero_ConElementos_VariosBloques_HaceLoMismoQueConDos() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();
			var bloque = _bloqueConMenosElementos;
			listBloques.Insertar(_bloqueConMenosElementos, 0);
			listBloques.Insertar(_bloqueConElementos, 1);
			listBloques.InsertarUltimo(123);
			var lleno = listBloques.GetBloque(listBloques.CantidadBloques - 1).Lleno;
			var cantidad = listBloques.CantidadBloques;
			var primero = bloque.PrimerElemento;
			var longitudInicial = listBloques.Longitud;

			// Act
			var result = listBloques.BorrarPrimero();

			// Assert
			Assert.Multiple(() => {
				Assert.That(lleno ^ cantidad == listBloques.CantidadBloques);
				Assert.That(primero, Is.EqualTo(result));
				Assert.That(listBloques.Longitud, Is.EqualTo(longitudInicial - 1));
			});
		}

		[Test]
		public void BorrarPrimero_SinElementos_LanzaUnaExcepcion() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();

			// Assert
			Assert.Throws<InvalidOperationException>(() => listBloques.BorrarPrimero());
		}

		[Test]
		public void BorrarTodos_ConElementos_VaciaLaLista() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			listBloques.InsertarUltimo(1);

			// Act
			listBloques.BorrarTodos();

			// Assert
			Assert.Multiple(() => {
				Assert.That(listBloques.Vacia);
				Assert.That(listBloques.Longitud, Is.EqualTo(0));
			});
		}

		[Test]
		public void BorrarTodos_SinElementos_VaciaLaLista() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();

			// Act
			listBloques.BorrarTodos();

			// Assert
			Assert.Multiple(() => {
				Assert.That(listBloques.Vacia);
				Assert.That(listBloques.Longitud, Is.EqualTo(0));
			});
		}

		[Test]
		public void BorrarTodos_Elemento_SinOcurrencias_NoBorraNadaYDevuelve0() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = 1234;
			listBloques.Insertar(_bloqueConElementos, 0);
			int longitud = listBloques.Longitud;

			// Act
			var result = listBloques.BorrarTodos(
				elemento);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result + listBloques.Longitud, Is.EqualTo(longitud));
				Assert.That(result, Is.EqualTo(0));
			});
		}

		//Falla por Insertar(B,int)
		[Test]
		public void BorrarTodos_Elemento_ConOcurrencias_LasBorraYDevuelveLaCantidad() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();
			int elemento = 1;
			listBloques.Insertar(_bloqueConElementos, 0);
			listBloques.Insertar(_bloqueConMenosElementos, 1);
			int longitud = listBloques.Longitud;

			// Act
			var result = listBloques.BorrarTodos(
				elemento);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result + listBloques.Longitud, Is.EqualTo(longitud));
				Assert.That(result, Is.GreaterThan(0));
				Assert.That(listBloques.Contiene(elemento), Is.False);
			});
		}

		[Test]
		public void BorrarUltimos_SinElElemento_NoBorraNadaYDevuelve0() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = 123;
			listBloques.Insertar(_bloqueConElementos, 0);
			listBloques.InsertarUltimo(elemento);

			// Act
			var result = listBloques.BorrarUltimos(
				elemento);

			// Assert
			Assert.That(result, Is.EqualTo(1));
		}

		[Test]
		public void BorrarVariosBloques_MasBloquesEnLista_LosBorraYDevuelveLaCantidad() {
			// Arrange
			int longitud = 100;
			var listBloques = new ListBloques<int,ArrayBloque<int>>(n => 10, n => n) {
				Longitud = longitud
			};
			int num = 3;
			int posicion = 5;
			var bloque = listBloques.GetBloque(8);

			// Act
			var result = listBloques.BorrarVariosBloques(
				num,
				posicion);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result, Is.EqualTo(num));
				Assert.That(listBloques.Longitud, Is.EqualTo(longitud - 10 * num));
				Assert.That(listBloques.Posicion(bloque), Is.GreaterThan(-1));
			});
		}

		[Test]
		public void BorrarVariosBloques_SinSuficientesBloques_LosBorraYDevuelveLaCantidad() {
			// Arrange
			int longitud = 100;
			var listBloques = new ListBloques<int, ArrayBloque<int>>(n => 10, n => n) {
				Longitud = longitud
			};
			int num = 3;
			int posicion = 8;

			// Act
			var result = listBloques.BorrarVariosBloques(
				num,
				posicion);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result, Is.LessThan(num));
				Assert.That(listBloques.Longitud, Is.EqualTo(longitud - 10 * result));
			});
		}

		[Test]
		public void BuscarBloque_Posicion_Valida_DevuelveLaPosicion() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>(n => 10, n => n) {
				Longitud = 100
			};
			int posicion = 50;

			// Act
			var result = listBloques.BuscarBloque(
				posicion);

			// Assert
			Assert.That(listBloques.GetBloque(result).Contiene(listBloques[posicion]), Is.True);
		}

		[TestCase(-1)]
		[TestCase(1000)]
		public void BuscarBloque_Posicion_NoValida_DevuelveM1(int value) {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>(n => 10, n => n) {
				Longitud = 100
			};
			int posicion = value;

			// Act
			var result = listBloques.BuscarBloque(
				posicion);

			// Assert
			Assert.That(result, Is.EqualTo(-1));
		}

		[Test]
		public void BuscarBloque_Elemento_EnLaLista_DevuelveLaPosicion() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>(n => 10, n => n) {
				Longitud = 100
			};
			int elemento = 15;

			// Act
			var result = listBloques.BuscarBloque(
				elemento);

			// Assert
			Assert.That(listBloques.GetBloque(result).Contiene(elemento));
		}

		[Test]
		public void BuscarBloque_Elemento_NoEnLaLista_DevuelveLaPosicion() {
			// Arrange
			var listBloques = new ListBloques<int?, ArrayBloque<int?>>(n => 10, n => 10) {
				Longitud = 100
			};
			int? elemento = 15;

			// Act
			var result = listBloques.BuscarBloque(
				elemento);

			// Assert
			Assert.That(result, Is.EqualTo(-1));
		}

		[Test]
		public void Clonar_Vacia_DevuelveListaIgual() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();

			// Act
			var result = listBloques.ClonarBloquesDinamica();

			Assert.Multiple(() => {
				// Assert
				Assert.That(ReferenceEquals(result, listBloques), Is.False);
				Assert.That(result, Is.EqualTo(listBloques));
			});
		}

		[Test]
		public void Clonar_ConElementos_DevuelveListaIgual() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>(n => 10, n => n) {
				Longitud = 100
			};

			// Act
			var result = listBloques.ClonarBloquesDinamica();

			Assert.Multiple(() => {
				// Assert
				Assert.That(ReferenceEquals(result, listBloques), Is.False);
				Assert.That(result, Is.EqualTo(listBloques));
			});
		}

		[Test]
		public void Contiene_ConElElemento_DevuelveTrue() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>(n => 10, n => n) {
				Longitud = 100
			};
			int elemento = 101;

			// Act
			var result = listBloques.Contiene(
				elemento);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public void Contiene_SinElElemento_DevuelveFalse() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>(n => 10, n => n) {
				Longitud = 100
			};
			int elemento = 1;

			// Act
			var result = listBloques.Contiene(
				elemento);

			// Assert
			Assert.That(result, Is.True);
		}

		[Test]
		public void Diferencia_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			ILista<int> lista = null;

			// Act
			var result = listBloques.Diferencia(
				lista);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Eliminar_Posicion_EnLista_BorraElElementoYLoDevuelve() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			listBloques.Insertar(_bloqueConElementos, 0);
			int posicion = new Random().Next(10), longitud = listBloques.Longitud;
			int elemento = listBloques[posicion];

			// Act
			var result = listBloques.Eliminar(
				posicion);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result, Is.EqualTo(elemento));
				Assert.That(longitud, Is.EqualTo(listBloques.Longitud + 1));
			});
		}

		[TestCase(-1)]
		[TestCase(100)]
		public void Eliminar_Posicion_NoEnLista_LanzaUnaExcepcion(int value) {
			// Arrange
			var listBloques = new ListBloques<int?, ArrayBloque<int?>>();
			listBloques.Insertar(new int?[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 0);
			int posicion = value, longitud = listBloques.Longitud;

			// Act
			Assert.Throws<ArgumentOutOfRangeException>(() => listBloques.Eliminar(posicion));
		}

		[Test]
		public void Eliminar_Elemento_EnLista_BorraElElementoYDevuelveSuPosicion() {
			// Arrange
			var listBloques = new ListBloques<int?, ArrayBloque<int?>>();
			listBloques.Insertar(new int?[5] {1,2,3,4,5}, 0);
			int posicion = 3, longitud = listBloques.Longitud;
			int? elemento = listBloques[posicion];

			// Act
			var result = listBloques.Eliminar(
				elemento);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result, Is.EqualTo(posicion));
				Assert.That(longitud, Is.EqualTo(listBloques.Longitud + 1));
				Assert.That(listBloques.Contiene(elemento), Is.False);
			});
		}

		[Test]
		public void Eliminar_Elemento_NoEnLaLista_NoBorraNadaYDevuelveM1() {
			// Arrange
			var listBloques = new ListBloques<int?, ArrayBloque<int?>>();
			int longitud = 5;
			listBloques.Insertar(new int?[5] { 1, 2, 3, 4, 5 }, 0);
			int? elemento = -1;

			// Act
			var result = listBloques.Eliminar(elemento);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result, Is.EqualTo(-1));
				Assert.That(longitud, Is.EqualTo(listBloques.Longitud));
			});
		}

		[Test]
		public void EliminarVarios_MitadDeLaLista_BorraLosElementosIndicadosYMantieneElResto() {
			// Arrange
			int longitud = 100;
			static int funcion(int n) => n;
			var listBloques = new ListBloques<int, ArrayBloque<int>>(n => 10, funcion) {
				Longitud = longitud
			};
			int num = 10;
			int posicion = 30;

			// Act
			var result = listBloques.EliminarVarios(
				num,
				posicion);

			// Assert
			Assert.That(longitud, Is.EqualTo(listBloques.Longitud + num));
			for (int i = 0; i < posicion; i++) {
				Assert.That(listBloques[i], Is.EqualTo(funcion(i)));
			}
			for (int i = posicion; i < listBloques.Longitud; i++) {
				Assert.That(listBloques[i], Is.EqualTo(funcion(i + num)));
			}
		}

		[Test]
		public void GetBloque_PosicionValida_DevuelveElBloque() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			var bloque = _bloqueConElementos;
			listBloques.Insertar(bloque, 0);
			int posicion = 0;

			// Act
			var result = listBloques.GetBloque(
				posicion);

			// Assert
			Assert.That(result, Is.EqualTo(bloque));
		}

		[Test]
		public void GetBloques_ConBloques_DevuelveLosBloquesDeLaLista() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>> (n => 10, n => n) {
				Longitud = 100
			};
			int posicion = 0;

			// Act
			var result = listBloques.GetBloques();

			// Assert
			foreach (var bloque in result) {
				Assert.That(bloque, Is.EqualTo(listBloques.GetBloque(posicion++)));
			}
		}

		[Test]
		public void GetBloques_UnBloque_TieneUnBloque() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>(n => 10, n => n);

			// Act
			var result = listBloques.GetBloques();
			var enumerator = result.GetEnumerator();

			// Assert
			Assert.That(enumerator.MoveNext(), Is.True);
			Assert.That(enumerator.MoveNext(), Is.False);
		}

		[Test]
		public void GetEnumerator_ConElementos_ContieneLosElementosDeLaLista() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>(N => 10, N => N) {
				Longitud = 100
			};
			int i = 0;

			// Act
			var result = listBloques.GetEnumerator();

			// Assert
			while (result.MoveNext()) {
				Assert.That(result.Current, Is.EqualTo(listBloques[i++]));
			}
		}

		[Test]
		public void GetEnumerator_SinElementos_NoContieneElementos() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>(N => 10, N => N);

			// Act
			var result = listBloques.GetEnumerator();

			// Assert
			Assert.That(result.MoveNext(), Is.False);
		}

		[Test]
		public void Insertar_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);
			int posicion = 0;

			// Act
			listBloques.Insertar(
				elemento,
				posicion);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Insertar_StateUnderTest_ExpectedBehavior1() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default;

			// Act
			var result = listBloques.Insertar(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Insertar_BloqueNoNulo_EnListaVacia_InsertaElBloque() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			ArrayBloque<int> bloque = _bloques[1];
			int posicion = 0;

			// Act
			listBloques.Insertar(
				bloque,
				posicion);

			// Assert
			Assert.Multiple(() => {
				Assert.That(listBloques.Vacia, Is.False);
				Assert.That(listBloques.CantidadBloques, Is.EqualTo(2));
				Assert.That(listBloques.Longitud, Is.EqualTo(_bloques[1].Longitud));
			});
		}

		[Test]
		public void Insertar_BloqueNoNulo_EnListaNoVacia_InsertaElBloque() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();
			ArrayBloque<int> bloque = _bloqueConMenosElementos;
			listBloques.Insertar(_bloqueConElementos, 0);
			int posicion = 1, cantidad = listBloques.CantidadBloques, longitud = listBloques.Longitud;

			// Act
			listBloques.Insertar(
				bloque,
				posicion);

			// Assert
			Assert.Multiple(() => {
				Assert.That(listBloques.Vacia, Is.False);
				Assert.That(listBloques.CantidadBloques, Is.EqualTo(cantidad + 1));
				Assert.That(listBloques.Longitud, Is.EqualTo(longitud + bloque.Longitud));
				Assert.DoesNotThrow(() => _ = listBloques[_bloqueConElementos.Longitud]);
			});
		}

		[Test]
		public void InsertarFin_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default;

			// Act
			listBloques.InsertarUltimo(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void InsertarInicio_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);

			// Act
			listBloques.InsertarPrimero(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void InsertarVarios_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);
			int num = 0;
			int posicion = 0;

			// Act
			listBloques.InsertarVarios(
				elemento,
				num,
				posicion);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void IntercambiarBloques_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int primero = 0;
			int segundo = 0;

			// Act
			listBloques.IntercambiarBloques(
				primero,
				segundo);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Invertir_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();

			// Act
			listBloques.Invertir();

			// Assert
			Assert.Fail();
		}

		public void Longitud_EntradaNegativa_LanzaExcepcion() {
			// Arrange
			static int funcion(int n) => (int)Math.Pow(n, 2);
			var listBloques = new ListBloques<int, ArrayBloque<int>>(n => 10, funcion);
			int ejemplo = 1234;
			listBloques.InsertarUltimo(ejemplo);

			// Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => listBloques.Longitud = -1);
		}

		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(100)]
		public void Longitud_EntradaNoNegativa_CambiaALaLongitud(int value) {
			// Arrange
			static int funcion(int n) => (int)Math.Pow(n, 2);
			var listBloques = new ListBloques<int, ArrayBloque<int>>(n => 10, funcion);
			int ejemplo = 1234;
			listBloques.InsertarUltimo(ejemplo);
			listBloques.InsertarUltimo(ejemplo);
			listBloques.InsertarUltimo(ejemplo);
			var longitud = listBloques.Longitud;

			// Act
			listBloques.Longitud = value;

			// Assert
			Assert.That(listBloques.Longitud, Is.EqualTo(value));
			if (value <= longitud) {
				foreach (var item in listBloques) {
					Assert.That(item, Is.EqualTo(ejemplo));
				}
			} else {
				for (int i = 0; i < value; i++) {
					Assert.That(listBloques[i], Is.EqualTo(ejemplo).Or.EqualTo(funcion(i)));
				}
			}
		}

		[Test]
		public void Multiplicar_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int factor = 0;

			// Act
			var result = listBloques.Multiplicar(
				factor);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Ocurrencias_ElementoRandomEnLista_DevuelveLasOcurrencias() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = new Random().Next(10);
			for (int i = 0; i < elemento; i++) {
				listBloques.InsertarUltimo(0);
				listBloques.InsertarUltimo(1);
				listBloques.InsertarUltimo(2);
				listBloques.InsertarUltimo(3);
				listBloques.InsertarUltimo(4);
				listBloques.InsertarUltimo(5);
				listBloques.InsertarUltimo(6);
				listBloques.InsertarUltimo(7);
				listBloques.InsertarUltimo(8);
				listBloques.InsertarUltimo(9);
			}

			// Act
			var result = listBloques.Ocurrencias(elemento);

			// Assert
			Assert.That(result, Has.Length.EqualTo(listBloques.Count(n => n == elemento)));
			Assert.That(result, Is.Ordered);
		}

		[Test]
		public void Posicion_Elemento_EnListaRandom_EncuentraLaPrimeraOcurrencia() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>(n => 10, n => new Random(1337).Next(n));
			int elemento = 5, longitud = 100;
			for (int i = 0; i < 10; i++) {
				listBloques.Longitud = longitud;
				Assert.That(listBloques.Longitud, Is.EqualTo(longitud));

				// Act
				var result = listBloques.Posicion(
					elemento);

				Assert.Multiple(() => {
					// Assert
					Assert.That(result, Is.LessThan(listBloques.Longitud));
					Assert.That(result == -1, Is.EqualTo(listBloques.Ocurrencias(elemento).Length == 0));
					if (result != -1) {
						Assert.That(listBloques[result], Is.EqualTo(elemento));
					}
					for (int i = 0; i < result; i++) { // Debe ser la primera ocurrencia
						Assert.That(listBloques[i], Is.Not.EqualTo(elemento));
					}
				});
				listBloques.Vacia = true;
			}
			
		}

		[Test]
		public void Posicion_Bloque_EnLista_DevuelveLaPosicionDelBloque() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>(n => 5, n => n) {
				Longitud = 100
			};
			ArrayBloque<int> bloque;

			for (int i = 0; i < listBloques.CantidadBloques; i++) {
				bloque = listBloques.GetBloque(i);
				Assert.That(listBloques.Posicion(bloque), Is.EqualTo(i));
			}
		}

		[Test]
		public void Posicion_Bloque_NoEnLista_DevuelveM1() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>(n => 5, n => n) {
				Longitud = 100
			};
			ArrayBloque<int> bloque = _bloqueVacio;

			Assert.That(listBloques.Posicion(bloque), Is.EqualTo(-1));
		}

		[Test]
		public void PrimerElemento_SinElementos_LanzaUnaExcepcion() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();

			// Act

			// Assert
			// ToString para que falle al llamar a PrimerElemento
			Assert.Throws<InvalidOperationException>(() => listBloques.PrimerElemento.ToString()); 
		}

		[Test]
		public void PrimerElemento_ConElementos_DevuelveElElemento() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();
			listBloques.Insertar(_bloqueConElementos, 0);

			// Act
			var result = listBloques.PrimerElemento;

			// Assert
			Assert.That(result, Is.EqualTo(listBloques[0]));
		}

		[Test]
		public void Restar_Elemento_EnLista_BorraTodasLasOcurrenciasEnNuevaLista() {
			// Arrange
			static int funcion(int n) => n % 10;
			int elemento = 5, longitud = 100;
			var listBloques = new ListBloques<int, ArrayBloque<int>>(n => 10, funcion) {
				Longitud = longitud
			};
			int ocurrencias = listBloques.Ocurrencias(elemento).Length;

			// Act
			var result = listBloques.Restar(
				elemento);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result.Longitud, Is.EqualTo(longitud - ocurrencias));
				Assert.That(result.Contiene(elemento), Is.False);
				Assert.That(listBloques.Longitud, Is.EqualTo(longitud));
			});
		}

		[Test]
		public void Restar_Bloque_BorraElBloqueEnNuevaLista() {
			// Arrange
			int longitud = 100, posicion = 5;
			var listBloques = new ListBloques<int,ArrayBloque<int>>(n => 10, n => n) {
				Longitud = longitud
			};
			ArrayBloque<int> bloque = listBloques.GetBloque(posicion);

			// Act
			var result = listBloques.Restar(
				bloque);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result.GetBloque(posicion), Is.Not.EqualTo(bloque));
				Assert.That(result.Longitud, Is.EqualTo(longitud - bloque.Longitud));
				Assert.That(listBloques.Longitud, Is.EqualTo(longitud));
			});
		}

		[Test]
		public void SetBloque_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			ArrayBloque<int> bloque = null;
			int posicion = 0;

			// Act
			var result = listBloques.SetBloque(
				bloque,
				posicion);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Sumar_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);

			// Act
			var result = listBloques.Sumar(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Sumar_StateUnderTest_ExpectedBehavior1() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			ArrayBloque<int> bloque = null;

			// Act
			var result = listBloques.Sumar(
				bloque);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void UltimoElemento_ConElementos_DevuelveElElemento() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			listBloques.Insertar(_bloqueConElementos, 0);
			int valor = 100;

			// Act
			var result = listBloques.UltimoElemento;

			// Assert
			Assert.That(result, Is.EqualTo(listBloques[listBloques.Longitud - 1]));
		}

		[Test]
		public void UltimoElemento_SinElementos_LanzaUnaExcepcion() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();

			// Act

			// Assert
			Assert.Throws<InvalidOperationException>(() => _ = listBloques.UltimoElemento);
		}

		[Test]
		public void Unir_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			ILista<int> segunda = null;

			// Act
			var result = listBloques.Unir(
				segunda);

			// Assert
			Assert.Fail();
		}
	}
}
