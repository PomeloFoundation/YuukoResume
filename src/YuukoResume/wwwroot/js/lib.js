/* Author: ThemeREX - ShiftCV HTML 1.1 */
/* Should you have any questions regarding the theme, please contact us via: http://themerex.net/?p=142/#tickets */

/*jQuery(document).ready(function(){
	//include Custom.JS
	 jQuery('head').append('<link rel="stylesheet" type="text/css" media="all" href="custom_tools/css/custom.css">');
	 jQuery('head').append('<link rel="stylesheet" type="text/css" media="all" href="custom_tools/css/colorpicker.css">');
	 jQuery('head').append('<script type="text/javascript" src="custom_tools/js/colorpicker.js"></script>');
	 jQuery('head').append('<script type="text/javascript" src="custom_tools/js/custom-block.js"></script>');
});*/

/*global jQuery:false */
var error_msg_box = null;
var googlemap_refreshed = false;

jQuery(window).load(function() {
	"use strict";
	if (window.location.hash==='#portfolio') {
		jQuery('#portfolio .section_header .section_title a').trigger('click');
	}
});


jQuery(document).ready(function(){
	
	jQuery(".sc_contact_form .enter").click(function (e) {
        userSubmitForm();
        e.preventDefault();
        return false;
    });

    empt = 'Name field can not be empty';
    to_lng = 'Too long name field';
    to_lng = 'Too long name field';
    empt_mail = 'Too short (or empty) email address';
    to_lng_mail = 'Too long email address';
    incor = 'Incorrect email address';
    mes_empt = 'message can not be empty';
    to_lng_mes = 'Too long message';

	// contact form Validate
	function userSubmitForm() {
		var error = formValidate(jQuery(".sc_contact_form form"), {
			error_message_show: true,
			error_message_time: 5000,
			error_message_class: "sc_infobox sc_infobox_style_error",
			error_fields_class: "error_fields_class",
			exit_after_first_error: false,
			rules: [{
				field: "username",
				min_length: {
					value: 1,
					message: empt
				},
				max_length: {
					value: 160,
					message: to_lng
				}
			}, {
				field: "email",
				min_length: {
					value: 7,
					message: empt_mail
				},
				max_length: {
					value: 60,
					message: to_lng_mail
				},
				mask: {
					value: "^([a-z0-9_\-]+\\.)*[a-z0-9_\\-]+@[a-z0-9_\-]+(\\.[a-z0-9_\-]+)*\\.[a-z]{2,6}$",
					message: incor
				}
			}, {
				field: "message",
				min_length: {
					value: 1,
					message: mes_empt
				},
				max_length: {
					value: 200,
					message: to_lng_mes
				}
			}]
		});
		if (!error) {
			var user_name = jQuery(".sc_contact_form #sc_contact_form_username").val();
			var user_email = jQuery(".sc_contact_form #sc_contact_form_email").val();
			var user_site = jQuery(".sc_contact_form #sc_contact_form_site").val();
			var user_msg = jQuery(".sc_contact_form #sc_contact_form_message").val();
			var data = {
				action: "submit_contact_form",
				nonce: "e1f9461bc9",
				user_name: user_name,
				user_email: user_email,
				user_site: user_site,
				user_msg: user_msg
			};
			jQuery.post("include/sendmail.php", data, userSubmitFormResponse, "text");
		}
	}
	
	function userSubmitFormResponse(response) {
		var rez = JSON.parse(response);
		jQuery(".sc_contact_form .result")
			.toggleClass("sc_infobox_style_error", false)
			.toggleClass("sc_infobox_style_success", false);
		if (rez.error == "") {
			jQuery(".sc_contact_form .result").addClass("sc_infobox_style_success").html("Your message sended!");
			setTimeout("jQuery('.sc_contact_form .result').fadeOut(); jQuery('.sc_contact_form form').get(0).reset();", 3000);
		} else {
			jQuery(".sc_contact_form .result").addClass("sc_infobox_style_error").html("Transmit failed! " + rez.error);
		}
		jQuery(".sc_contact_form .result").fadeIn();
	}

		// toTop link setup
		"use strict";
		jQuery(window).scroll(function() {
			if(jQuery(this).scrollTop() >= 110) {
				jQuery('#toTop').show();	
			} else {
				jQuery('#toTop').hide();	
			}
		});
		jQuery('#toTop').click(function(e) {
			jQuery('body,html').animate({scrollTop:0}, 800);
			e.preventDefault();
		});

		// Pretty photo
		jQuery("a[href$='jpg'],a[href$='jpeg'],a[href$='png'],a[href$='gif']").attr('data-photo', 'prettyPhoto')
		jQuery("a[data-photo^='prettyPhoto']").click(function(e) {
			if (jQuery(window).width()<480)	{
				e.stopImmediatePropagation();
				window.location = jQuery(this).attr('href');
			}
			e.preventDefault();
			return false;
		});
		jQuery("a[data-photo^='prettyPhoto']").prettyPhoto({
			social_tools: '',
			theme: 'light_rounded'
		}); 


		// Section tabs
		jQuery('#mainpage_accordion_area').tabs('section > .section_body', {
			tabs: 'section > .section_header > .section_title',
			effect : 'slide',
			slideUpSpeed: 600,
			slideDownSpeed: 600,
			onClick: function (e, tabIndex) {
				var tabs = jQuery('#mainpage_accordion_area section > .section_header > .section_title');
				var tab = tabs.eq(tabIndex);
				
				if (tab.hasClass('resume_section_title')) {					// Resume
					jQuery('.widget_skills .skills_row').each(function(){
						var wd = jQuery(this).find('.progress').attr('data-process');
						if(jQuery(this).find('.progress').width() === 0) {
							jQuery(this).find('.progress').animate({'width': wd}, 700);
						}
						jQuery('.svg').addClass('vis');
					});
					if(jQuery('#resume .section_body').css('display') === 'none'){
						jQuery('#resume .section_body').parent().removeClass('open');
					}
					else {
						jQuery('#resume .section_body').parent().addClass('open');
					}
				} else if (tab.hasClass('portfolio_section_title')) {		// Portfolio
					// Isotope refresh
					if (jQuery('.portfolio_items.isotope').length > 0 && jQuery('.portfolio_items.isotope:hidden').length === 0) {
						jQuery('.portfolio_items').isotope({ filter: getIsotopeFilter() });}
				} else if (tab.hasClass('contact_section_title')) {		
				}
				return false;
			},
			currentClose: true,
			anotherClose: false,
			initialIndex: -1
		});

		jQuery('.cleared .widget_skills .skills_row').each(function(){
			var wd = jQuery(this).find('.progress').attr('data-process');
			if(jQuery(this).find('.progress').width() === 0) {
				jQuery(this).find('.progress').css({'width': wd}, 700);
			}
			jQuery('.svg').addClass('vis');
		});
		jQuery('#profile:not(.printable) .profile_section_header h2').click(function(){
			if (jQuery(this).find('.section_name').hasClass('show')){
				jQuery(this).find('.section_name').animate({'width':'135', 'opacity':'1'},
					550, 'easeOutCubic').removeClass('show');
			} else {
				jQuery(this).find('.section_name').animate({'width':'0', 'opacity':'0'},
					250,'easeOutCubic').slideDown().addClass('show');
			}
			jQuery(this).parent().toggleClass('opened').next('.profile_section_body').stop().slideToggle({
				duration: 450, easing: 'easeOutCubic'});
			return false;
		});
		
		jQuery('#mainpage_accordion_area h2.section_title').click(function(){
			var ofs = jQuery(this).offset().top;
			jQuery('html, body').animate({'scrollTop':ofs-50});
		});
		
		// Galleries Slider
		jQuery('.slider_container').flexslider({
			directionNav: true,
			controlNav: false
		});

		// ----------------------- Shortcodes setup -------------------
		jQuery('div.sc_infobox_closeable').click(function() {
			jQuery(this).fadeOut();
		});

		jQuery('.sc_tooltip_parent').hover(function(){
			var obj = jQuery(this);
			obj.find('.sc_tooltip').stop().animate({'marginTop': '5'}, 100).show();
		},
		function(){
			var obj = jQuery(this);
			obj.find('.sc_tooltip').stop().animate({'marginTop': '0'}, 100).hide();
		});
		

		// ----------------------- Comment form submit ----------------
		jQuery("form#commentform").submit(function(e) {
			var error = formValidate(jQuery(this), {
				error_message_text: 'Global error text',	// Global error message text (if don't write in checked field)
				error_message_show: true,			// Display or not error message
				error_message_time: 5000,			// Time to display error message
				error_message_class: 'sc_infobox sc_infobox_style_error',	// Class, appended to error message block
				error_fields_class: 'error_fields_class',			// Class, appended to error fields
				exit_after_first_error: false,			// Cancel validation and exit after first error
				rules: [
					{
						field: 'author',
						min_length: { value: 1,	 message: empt },
						max_length: { value: 160, message: to_lng}
					},
					{
						field: 'email',
						min_length: { value: 7,	 message: empt_mail },
						max_length: { value: 60, message: to_lng_mail},
						mask: { value: "^([a-z0-9_\\-]+\\\.)*[a-z0-9_\\\-]+@[a-z0-9_\\-]+(\\\.[a-z0-9_\\-]+)*\\\.[a-z]{2,6}$", message: incor}
					},
					{
						field: 'comment',
						min_length: { value: 1,  message: mes_empt },
						max_length: { value: 200, message: to_lng_mes}
					}
				]
			});
			if (error) {e.preventDefault();}
			return !error;
		});
		
		// Background theme
		jQuery('#theme_switcher').click(function(e) {
			var $body = jQuery(document).find('body').eq(0);
			var is_dark = $body.hasClass('dark');
			var theme_style = '';
			if (is_dark) {
				theme_style = 'light';
				jQuery(this).find('.switch_wrap').html('Dark version');
				$body.removeClass('dark').addClass('light');
				setStateStyleSheet('dark', false);
			} else {
				theme_style = 'dark';
				jQuery(this).find('.switch_wrap').html('Light version');
				$body.addClass('dark').removeClass('light');
				setStateStyleSheet('dark', true);
			}
			e.preventDefault();
			return false;
		});
		
		jQuery(window).scroll(function(){
			"use strict";
			if(jQuery('#resume').length === 0) {
				return;
			}
			var top = jQuery(document).scrollTop();
			if(jQuery('#resume').offset().top-60 < top || parseInt(jQuery('#resume_buttons').css('top'), 10) > 0) {
				var pr_h = jQuery('#resume_buttons').parent().height()-60;
				top = Math.min(pr_h, Math.max(0, top-jQuery('#resume').offset().top+50));
				jQuery('#resume_buttons').css({'top':top});
			}
		});
		
		


});



/* ============================================== */ 


/* Isotope init */
var curIsotopeFilter = '*';
var curIsotopePage = '';
jQuery(document).ready(function() {
	if(jQuery('.portfolio_items').length !== 0) {
		jQuery('.portfolio_items')
			.isotope({ 
				itemSelector: '.portfolio_post',
				transformsEnabled : true,
				duration: 750,
				resizable: true,
				resizesContainer: true,
				layoutMode: 'fitRows'
			});
		jQuery('.portfolio_items').css('height', '220px').find('article').css('transform' ,'none');
		jQuery('#portfolio_iso_filters li a').click(function(){
			var selector = jQuery(this).attr('data-filter');
			curIsotopeFilter = selector;
			pagesClear();
			pagesBuild();
			jQuery('.portfolio_items').isotope({ filter: getIsotopeFilter() });
			jQuery(this).parents('#portfolio_iso_filters').find('a').removeClass('current');
			jQuery(this).addClass('current');
			return false;
		});
		jQuery('#portfolio_iso_pages').on('click', 'li a', function(){
			var selector = jQuery(this).attr('data-filter');
			curIsotopePage = selector;
			jQuery('#portfolio_iso_pages_current').html(selector.substr(selector.lastIndexOf('_')+1));
			jQuery('.portfolio_items').isotope({ filter: getIsotopeFilter() });
			jQuery(this).parents('#portfolio_iso_pages').find('a').removeClass('current');
			jQuery(this).addClass('current');
			return false;
		});
		pagesBuild();
	}	
});
function getIsotopeFilter() {
	var flt = curIsotopeFilter!='*' ? curIsotopeFilter : '';
	flt += curIsotopePage!='' ? ((flt!='' ? '' : '') + curIsotopePage) : '';
	flt=='' ? '*' : '';
	return flt;
}
function pagesBuild() {
	var selector = '.portfolio_items article'+(curIsotopeFilter!='*' ? curIsotopeFilter : '');
	var items = jQuery(selector);
	var total = items.length;
	jQuery(".portfolio_iso_pages").hide();
	if (total > ppp) {
		var pagesList = '';
		var pagesTotal = Math.ceil(total/ppp);
		for (var i=1; i<=pagesTotal; i++)
			pagesList += '<li><a href="#" data-filter=".page_' + i + '"' + (i==1 ? ' class="current"' : '') + '>' + i + '</a></li>';
		items.each(function(idx, obj) {
			var pg = Math.floor(idx/ppp)+1;
			jQuery(obj).attr('data-page', pg).addClass('page_'+pg);
		});
		jQuery(".portfolio_iso_pages").show();
		jQuery("#portfolio_iso_pages").html(pagesList);
		jQuery("#portfolio_iso_pages_current").html("1");
		jQuery("#portfolio_iso_pages_total").html(pagesTotal);
		curIsotopePage = '.page_1';
	}
}
function pagesClear() {
	jQuery('.portfolio_items article').each(function (idx, obj) {
		var pg = jQuery(obj).attr('data-page');
		if (pg > 0) {
			jQuery(obj).attr('data-page', '').removeClass('page_'+pg);
		}
	});
	jQuery(".portfolio_iso_pages").hide();
	curIsotopePage = '';
}
function hideCommentScroll() {
	var com_top = jQuery('#comments').offset().top;
	var win_top = jQuery(window).scrollTop();
	var win_ht = jQuery(window).height();
	if((win_top + win_ht)-200 > com_top){
		jQuery('#scrollTo').hide();
	}
	else {
		jQuery('#scrollTo').show();
	}
}

jQuery(window).load(function(){
	if(jQuery('#comments').length > 0) {
	hideCommentScroll();
	jQuery('#scrollTo').click(function(){
		var target = jQuery(this).attr('href');
		var ofs = jQuery(target).offset().top;
		jQuery('html, body').animate({scrollTop : ofs-150});
	});
	jQuery(window).scroll(function(){
		hideCommentScroll();
	});
	}
});

	/*global jQuery:false */
	var error_msg_box = null;
	var googlemap_refreshed = false;

	jQuery(window).load(function() {
		"use strict";
		if (window.location.hash==='#portfolio') {
			jQuery('#portfolio .section_header .section_title a').trigger('click');
		}
	});




/* Isotope init */
var curIsotopeFilter = '*';
var curIsotopePage = '';
jQuery(document).ready(function() {
	if(jQuery('.portfolio_items').length !== 0) {
		jQuery('.portfolio_items')
			.isotope({ 
				itemSelector: '.portfolio_post',
				transformsEnabled : true,
				duration: 750,
				resizable: true,
				resizesContainer: true,
				layoutMode: 'fitRows'
			});
		jQuery('.portfolio_items').css('height', '220px').find('article').css('transform' ,'none');
		jQuery('#portfolio_iso_filters li a').click(function(){
			var selector = jQuery(this).attr('data-filter');
			curIsotopeFilter = selector;
			pagesClear();
			pagesBuild();
			jQuery('.portfolio_items').isotope({ filter: getIsotopeFilter() });
			jQuery(this).parents('#portfolio_iso_filters').find('a').removeClass('current');
			jQuery(this).addClass('current');
			return false;
		});
		jQuery('#portfolio_iso_pages').on('click', 'li a', function(){
			var selector = jQuery(this).attr('data-filter');
			curIsotopePage = selector;
			jQuery('#portfolio_iso_pages_current').html(selector.substr(selector.lastIndexOf('_')+1));
			jQuery('.portfolio_items').isotope({ filter: getIsotopeFilter() });
			jQuery(this).parents('#portfolio_iso_pages').find('a').removeClass('current');
			jQuery(this).addClass('current');
			return false;
		});
		pagesBuild();
	}	
});
function getIsotopeFilter() {
	var flt = curIsotopeFilter!='*' ? curIsotopeFilter : '';
	flt += curIsotopePage!='' ? ((flt!='' ? '' : '') + curIsotopePage) : '';
	flt=='' ? '*' : '';
	return flt;
}
function pagesBuild() {
	var selector = '.portfolio_items article'+(curIsotopeFilter!='*' ? curIsotopeFilter : '');
	var items = jQuery(selector);
	var total = items.length;
	jQuery(".portfolio_iso_pages").hide();
	var ppp = 9;
	if (total > ppp) {
		var pagesList = '';
		var pagesTotal = Math.ceil(total/ppp);
		for (var i=1; i<=pagesTotal; i++)
			pagesList += '<li><a href="#" data-filter=".page_' + i + '"' + (i==1 ? ' class="current"' : '') + '>' + i + '</a></li>';
		items.each(function(idx, obj) {
			var pg = Math.floor(idx/ppp)+1;
			jQuery(obj).attr('data-page', pg).addClass('page_'+pg);
		});
		jQuery(".portfolio_iso_pages").show();
		jQuery("#portfolio_iso_pages").html(pagesList);
		jQuery("#portfolio_iso_pages_current").html("1");
		jQuery("#portfolio_iso_pages_total").html(pagesTotal);
		curIsotopePage = '.page_1';
	}
}
function pagesClear() {
	jQuery('.portfolio_items article').each(function (idx, obj) {
		var pg = jQuery(obj).attr('data-page');
		if (pg > 0) {
			jQuery(obj).attr('data-page', '').removeClass('page_'+pg);
		}
	});
	jQuery(".portfolio_iso_pages").hide();
	curIsotopePage = '';
}
function hideCommentScroll() {
	var com_top = jQuery('#comments').offset().top;
	var win_top = jQuery(window).scrollTop();
	var win_ht = jQuery(window).height();
	if((win_top + win_ht)-200 > com_top){
		jQuery('#scrollTo').hide();
	}
	else {
		jQuery('#scrollTo').show();
	}
}

jQuery(window).load(function(){
	if(jQuery('#comments').length > 0) {
	hideCommentScroll();
	jQuery('#scrollTo').click(function(){
		var target = jQuery(this).attr('href');
		var ofs = jQuery(target).offset().top;
		jQuery('html, body').animate({scrollTop : ofs-150});
	});
	jQuery(window).scroll(function(){
		hideCommentScroll();
	});
	}
});


















