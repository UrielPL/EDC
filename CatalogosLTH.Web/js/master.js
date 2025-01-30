$(function(){
	$('.bxslider').bxSlider({ auto: true, pause: 7000, speed: 1000 });
	$(".owl-carousel").owlCarousel({
		itemsCustom:[
						[300, 3],
						[400, 3], 
						[550, 4],
						[750, 5],
						[1000, 6]
					],
		autoPlay: 5000,
		scrollPerPage: true
	});
	$('.fancybox').fancybox({autoSize: 'false', maxWidth:600 });
	$(".forma-contacto").submit(function(e){
		e.preventDefault();
		valido = true;
		
		$(this).find('input').each(function(){
			if($(this).prop('required')){
				if($(this).val().length === 0){
					valido = false;
				}
			}
		});
		
		if(valido){
			$(this).find('input[type="submit"]').val('Enviando...');
			$(this).find('input[type="submit"]').attr('disabled','disabled');
			$.post("php/envia.php", $(this).serialize(), function(data) {
				if(data.msj=="OK"){
					location.href = data.url;
				}
				else{
					alert("Ocurrió un error al enviar el formulario, intenta de nuevo");
					$(this).find('input[type="submit"]').removeAttr('disabled');
					$(this).find('input[type="submit"]').val('Enviar');
				}
			}, 'json');
		}
		else{
			alert("Favor de completar los campos obligatorios");
		}
	});
});