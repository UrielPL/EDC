$(document).ready(function(){
	var height = ($(window).height() / 2);
	var scroll = $(window).scrollTop() + height;
	var parallax = "text-parallax";
	var bibliotecas = $("#bibliotecas").offset().top;
	var profesionalizacion = $("#profesionalizacion").offset().top;
	var planNegocio = $("#plan-negocio").offset().top;
	var actividades = $("#actividades").offset().top;

	/*$("#sizes").html("Scroll: " + scroll + 
	    				 "<br>Bibliotecas: " + bibliotecas + 
					     "<br>Profesionalización: " + profesionalizacion +
					     "<br>Plan Negocio: " + planNegocio +
					     "<br>Actividades: " + actividades);*/
	scrolls();

	$(window).scroll(function (event) {   
	    /*$("#sizes").html("Scroll: " + scroll + 
	    				 "<br>Bibliotecas: " + bibliotecas + 
					     "<br>Profesionalización: " + profesionalizacion +
					     "<br>Plan Negocio: " + planNegocio +
					     "<br>Actividades: " + actividades);*/

	    scrolls();	    
	});

	function scrolls() {
		var scroll = $(window).scrollTop() + height;
		if (scroll >= bibliotecas) {
	    	$('#bibliotecas .parallax').addClass('animation');
	    }
	    if (scroll >= profesionalizacion) {
	    	$('#profesionalizacion .parallax').addClass('animation');
	    }
	    if (scroll >= planNegocio) {
	    	$('#plan-negocio .parallax').addClass('animation');
	    }
	    if (scroll >= actividades) {
	    	$('#actividades .parallax').addClass('animation');
	    }
	}
});