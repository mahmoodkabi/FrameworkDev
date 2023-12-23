
(function ($) {
    $(document).ready(function () {
        var shp = $('#show-password');

        // Show Password
        shp.click(function () {
            var tb = $('#' + $(this).attr('for'));

            if ($(this).hasClass('fa-eye')) {
                tb.attr('type', 'text');
                $(this).removeClass('fa-eye');
                $(this).addClass('fa-eye-slash');
            } else {
                tb.attr('type', 'password');
                $(this).removeClass('fa-eye-slash');
                $(this).addClass('fa-eye');
            }
        })
    });
})(jQuery);

if (window.addEventListener)
    window.addEventListener("load", replace_placeholder);
else if (window.attachEvent)
    window.attachEvent("onload", replace_placeholder);
else
    window.onload = replace_placeholder;

function replace_placeholder() {
    var test_input = document.createElement('input');

    if (test_input.placeholder == undefined) {
        var inputs = document.getElementsByTagName('input');

        for (i = 0; i <= inputs.length; i++) {
            var input = inputs[i];
            if (input == undefined)
                continue;

            if (input.hasAttribute('placeholder')) {
                var placeholder_text = input.getAttribute('placeholder'),
                    input_type = input.getAttribute('type');

                input.value = placeholder_text;
                input.placeholder_text = placeholder_text;
                input.input_type = input_type;

                if (input_type == 'password')
                    input.setAttribute('type', 'text');

                var focus_function = function (e) {
                    if (this.value == this.placeholder_text) {
                        this.value = '';
                    }

                    if (this.input_type == 'password') {
                        this.setAttribute('type', 'password');
                    }
                }
                addEvent(input, 'focus', focus_function);

                var blur_function = function (e) {
                    if (this.value == '') {
                        this.value = this.placeholder_text;
                    }
                    if (this.input_type == 'password') {
                        this.setAttribute('type', 'text');
                    }
                }

                addEvent(input, 'blur', blur_function);
            }
        }
    }
}

function addEvent(elm, type, fn) {
    if (elm.attachEvent) {
        elm['e' + type + fn] = fn;
        elm[type + fn] = function () { elm['e' + type + fn](window.event); }
        elm.attachEvent('on' + type, elm[type + fn]);
    } else {
        elm.addEventListener(type, fn, false);
    }
}

function removeEvent() {
    if (obj.detachEvent) {
        obj.detachEvent('on' + type, obj[type + fn]);
        obj[type + fn] = null;
    } else {
        obj.removeEventListener(type, fn, false);
    }
}
