function Util(){}Util.hasClass=function(t,e){return t.classList?t.classList.contains(e):!!t.getAttribute("class").match(new RegExp("(\\s|^)"+e+"(\\s|$)"))},Util.addClass=function(t,e){e=e.split(" ");t.classList?t.classList.add(e[0]):Util.hasClass(t,e[0])||t.setAttribute("class",t.getAttribute("class")+" "+e[0]),1<e.length&&Util.addClass(t,e.slice(1).join(" "))},Util.removeClass=function(t,e){var n=e.split(" ");t.classList?t.classList.remove(n[0]):Util.hasClass(t,n[0])&&(e=new RegExp("(\\s|^)"+n[0]+"(\\s|$)"),t.setAttribute("class",t.getAttribute("class").replace(e," "))),1<n.length&&Util.removeClass(t,n.slice(1).join(" "))},Util.toggleClass=function(t,e,n){n?Util.addClass(t,e):Util.removeClass(t,e)},Util.setAttributes=function(t,e){for(var n in e)t.setAttribute(n,e[n])},Util.getChildrenByClassName=function(t,e){t.children;for(var n=[],s=0;s<t.children.length;s++)Util.hasClass(t.children[s],e)&&n.push(t.children[s]);return n},Util.is=function(t,e){if(e.nodeType)return t===e;for(var n="string"==typeof e?document.querySelectorAll(e):e,s=n.length;s--;)if(n[s]===t)return!0;return!1},Util.setHeight=function(n,s,i,a,o,l){var r=s-n,c=null,u=function(t){var e=t-(c=c||t);a<e&&(e=a);t=parseInt(e/a*r+n);l&&(t=Math[l](e,n,s-n,a)),i.style.height=t+"px",e<a?window.requestAnimationFrame(u):o&&o()};i.style.height=n+"px",window.requestAnimationFrame(u)},Util.scrollTo=function(n,s,i,t){var a=t||window,o=a.scrollTop||document.documentElement.scrollTop,l=null;t||(o=window.scrollY||document.documentElement.scrollTop);var r=function(t){var e=t-(l=l||t);s<e&&(e=s);t=Math.easeInOutQuad(e,o,n-o,s);a.scrollTo(0,t),e<s?window.requestAnimationFrame(r):i&&i()};window.requestAnimationFrame(r)},Util.moveFocus=function(t){(t=t||document.getElementsByTagName("body")[0]).focus(),document.activeElement!==t&&(t.setAttribute("tabindex","-1"),t.focus())},Util.getIndexInArray=function(t,e){return Array.prototype.indexOf.call(t,e)},Util.cssSupports=function(t,e){return"CSS"in window?CSS.supports(t,e):t.replace(/-([a-z])/g,function(t){return t[1].toUpperCase()})in document.body.style},Util.extend=function(){var n={},s=!1,t=0,e=arguments.length;"[object Boolean]"===Object.prototype.toString.call(arguments[0])&&(s=arguments[0],t++);for(;t<e;t++)!function(t){for(var e in t)Object.prototype.hasOwnProperty.call(t,e)&&(s&&"[object Object]"===Object.prototype.toString.call(t[e])?n[e]=extend(!0,n[e],t[e]):n[e]=t[e])}(arguments[t]);return n},Util.osHasReducedMotion=function(){if(!window.matchMedia)return!1;var t=window.matchMedia("(prefers-reduced-motion: reduce)");return!!t&&t.matches},Element.prototype.matches||(Element.prototype.matches=Element.prototype.msMatchesSelector||Element.prototype.webkitMatchesSelector),Element.prototype.closest||(Element.prototype.closest=function(t){var e=this;if(!document.documentElement.contains(e))return null;do{if(e.matches(t))return e}while(null!==(e=e.parentElement||e.parentNode)&&1===e.nodeType);return null});{function CustomEvent(t,e){e=e||{bubbles:!1,cancelable:!1,detail:void 0};var n=document.createEvent("CustomEvent");return n.initCustomEvent(t,e.bubbles,e.cancelable,e.detail),n}"function"!=typeof window.CustomEvent&&(CustomEvent.prototype=window.Event.prototype,window.CustomEvent=CustomEvent)}function resetFocusTabsStyle(){window.dispatchEvent(new CustomEvent("initFocusTabs"))}function putCursorAtEnd(t){var e;t.setSelectionRange?(e=2*t.value.length,t.focus(),t.setSelectionRange(e,e)):t.value=t.value}function onLogin(){let n={email:document.getElementById("login-email").value,password:document.getElementById("login-password").value};(async()=>{const t=await fetch("https://api.oasisplatform.world/api/avatar/authenticate",{method:"POST",body:JSON.stringify(n),headers:{"Content-Type":"application/json"}});var e;200===t.status?(e=await t.json(),alert(e.message)):(e=await t.json(),alert(e.title)),window.location.reload()})()}function onSignup(){let n={email:document.getElementById("signup-email").value,password:document.getElementById("signup-password").value,confirmPassword:document.getElementById("confirm-signup-password").value,acceptTerms:!0,avatarType:"User"};(async()=>{const t=await fetch("https://api.oasisplatform.world/api/avatar/register",{method:"POST",body:JSON.stringify(n),headers:{"Content-Type":"application/json"}});var e;200===t.status?(e=await t.json(),alert(e.message)):(e=await t.json(),alert(e.title)),window.location.reload()})()}Math.easeInOutQuad=function(t,e,n,s){return(t/=s/2)<1?n/2*t*t+e:-n/2*(--t*(t-2)-1)+e},Math.easeInQuart=function(t,e,n,s){return n*(t/=s)*t*t*t+e},Math.easeOutQuart=function(t,e,n,s){return t/=s,-n*(--t*t*t*t-1)+e},Math.easeInOutQuart=function(t,e,n,s){return(t/=s/2)<1?n/2*t*t*t*t+e:-n/2*((t-=2)*t*t*t-2)+e},Math.easeOutElastic=function(t,e,n,s){var i=1.70158,a=.7*s,o=n;return 0==t?e:1==(t/=s)?e+n:(a=a||.3*s,i=o<Math.abs(n)?(o=n,a/4):a/(2*Math.PI)*Math.asin(n/o),o*Math.pow(2,-10*t)*Math.sin((t*s-i)*(2*Math.PI)/a)+n+e)},function(){var s=document.getElementsByClassName("js-tab-focus"),t=!1,e=!1,n=!1;function i(){0<s.length&&(o(!1),window.addEventListener("keydown",a)),window.removeEventListener("mousedown",i),n=!(e=!1)}function a(t){9===t.keyCode&&(o(!0),window.removeEventListener("keydown",a),window.addEventListener("mousedown",i),e=!0)}function o(t){for(var e=t?"":"none",n=0;n<s.length;n++)s[n].style.setProperty("outline",e)}function l(){t?n&&o(e):(t=0<s.length,window.addEventListener("mousedown",i))}l(),window.addEventListener("initFocusTabs",l)}(),function(){function t(t){this.element=t,this.password=this.element.getElementsByClassName("js-password__input")[0],this.visibilityBtn=this.element.getElementsByClassName("js-password__btn")[0],this.visibilityClass="password--text-is-visible",this.initPassword()}t.prototype.initPassword=function(){var e=this;this.visibilityBtn.addEventListener("click",function(t){document.activeElement!==e.password&&(t.preventDefault(),e.togglePasswordVisibility())})},t.prototype.togglePasswordVisibility=function(){var t=!Util.hasClass(this.element,this.visibilityClass);Util.toggleClass(this.element,this.visibilityClass,t),t?this.password.setAttribute("type","text"):this.password.setAttribute("type","password")};var e=document.getElementsByClassName("js-password");if(0<e.length)for(var n=0;n<e.length;n++)new t(e[n])}(),function(){var t=document.getElementsByClassName("js-search");if(0<t.length)for(var e=0;e<t.length;e++)t[e].getElementsByClassName("js-search__input")[0].addEventListener("input",function(t){Util.toggleClass(t.target,"search__input--has-content",0<t.target.value.length)})}(),jQuery(document).ready(function(e){var n=e(".js-nav-trigger"),s=(e(".main-content"),e("body"));$navigation=e(".side-nav"),n.on("click",function(t){t.preventDefault(),n.toggleClass("is-clicked"),$navigation.toggleClass("is-visible"),e(".item--has-children").children("a").removeClass("submenu-open").next(".sub-menu").delay(300).slideUp()}),s.on("click",function(t){t.preventDefault(),(e(t.target).is(s)||e(t.which).is("27"))&&(n.removeClass("is-clicked"),$navigation.removeClass("is-visible"),e(".item--has-children").children("a").removeClass("submenu-open").next(".sub-menu").delay(300).slideUp())}),s.on("keydown",function(t){t.preventDefault(),e(t.which).is("27")&&(n.removeClass("is-clicked"),$navigation.removeClass("is-visible"),e(".item--has-children").children("a").removeClass("submenu-open").next(".sub-menu").delay(300).slideUp())}),e(".item--has-children").children("a").on("click",function(t){t.preventDefault(),e(this).toggleClass("submenu-open").next(".sub-menu").slideToggle(300).end().parent(".item--has-children").siblings(".item--has-children").children("a").removeClass("submenu-open").next(".sub-menu").slideUp(300)})}),jQuery(document).ready(function(t){function e(t){this.element=t,this.blocks=document.getElementsByClassName("js-modal-block"),this.triggers=document.getElementsByClassName("js-modal-trigger"),this.init()}e.prototype.init=function(){for(var t,e=this,n=0;n<this.triggers.length;n++)t=n,e.triggers[t].addEventListener("click",function(t){t.target.hasAttribute("data-signin")&&(t.preventDefault(),e.showSigninForm(t.target.getAttribute("data-signin")))});this.element.addEventListener("click",function(t){(s(t.target,"js-modal")||s(t.target,"js-close"))&&(t.preventDefault(),a(e.element,"is-visible"))}),document.addEventListener("keydown",function(t){"27"==t.which&&a(e.element,"is-visible")})},e.prototype.showSigninForm=function(t){s(this.element,"is-visible")||i(this.element,"is-visible");for(var e=0;e<this.blocks.length;e++)(this.blocks[e].getAttribute("data-type")==t?i:a)(this.blocks[e],"is-selected")},e.prototype.toggleError=function(t,e){o(t,"modal__input--has-error",e),o(t.nextElementSibling,"modal__error--is-visible",e)};var n=document.getElementsByClassName("js-modal")[0];function s(t,e){return t.classList?t.classList.contains(e):t.className.match(new RegExp("(\\s|^)"+e+"(\\s|$)"))}function i(t,e){e=e.split(" ");t.classList?t.classList.add(e[0]):s(t,e[0])||(t.className+=" "+e[0]),1<e.length&&i(t,e.slice(1).join(" "))}function a(t,e){var n=e.split(" ");t.classList?t.classList.remove(n[0]):s(t,n[0])&&(e=new RegExp("(\\s|^)"+n[0]+"(\\s|$)"),t.className=t.className.replace(e," ")),1<n.length&&a(t,n.slice(1).join(" "))}function o(t,e,n){(n?i:a)(t,e)}n&&new e(n)});
//# sourceMappingURL=scripts.js.map
