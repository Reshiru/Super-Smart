window.onload = () => {

    //save akk forms and inputs in a variable
    let forms = document.getElementsByTagName('form');
    let inp = document.getElementsByTagName('input');
    let textareas = document.querySelectorAll("textarea");

    //disable default validation on input
    $('input').on("invalid", function (e) {
        e.preventDefault();
        e.stopPropagation();
        validateInput($(this)[0]);
    });

    //disable default validation on textarea
    $('textarea').on("invalid", function (e) {
        e.preventDefault();
        e.stopPropagation();
        validateTextarea($(this)[0]);
    });

    //add after every input a span with the css class error message
    for (let input of inp) {
        input.setAttribute("data-validationtextid", guid());
        input.setAttribute("novalidate", "novalidate");
        input.insertAdjacentHTML('afterend', "<span id='" + input.getAttribute("data-validationtextid") + "' class='errormessage'></span>");
    }

    //add after every textarea a span with the css class error message
    for (let textA of textareas) {
        textA.setAttribute("data-validationtextid", guid());
        textA.setAttribute("novalidate", "novalidate");
        textA.insertAdjacentHTML('afterend', "<span id='" + textA.getAttribute("data-validationtextid") + "' class='errormessage'></span>");
    }

    //validates each form on submit
    for (let form of forms) {
        form.onsubmit = function (e) {
            if (!allInputsValid(form)) {
                e.preventDefault();
                for (let input of inp) {
                    if (input.from === form) {
                        validateInput(input);
                    }
                }

                for (let textA of textareas) {
                    if (textA.form === form) {
                        validateTextarea(textA);
                    }
                }
            }
        }
    }

    //add key down validation
    for (let input of inp) {
        let form = input.form;

        input.onkeyup = (e) => {
            validateInput(input);
        }
    }

    //add key down validation
    for (let textA of textareas) {
        textA.onkeyup = (e) => {
            validateTextarea(textA);
        }
    }

    //returns guid
    function guid() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    }

    //validates email => returns true or false
    function validateEmail(email) {
        const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(email).toLowerCase());
    }

    //validates input and reacts on input value
    function validateInput(input) {
        let form = input.form;
        if (input.type === "email") {
            if (validateEmail(input.value)) {
                input.classList.remove("wrongEmail");
                input.classList.add("validateEmail");
                document.getElementById(input.getAttribute('data-validationtextid')).innerHTML = "";
            } else {
                input.classList.remove("validateEmail");
                input.classList.add("wrongEmail");
                document.getElementById(input.getAttribute('data-validationtextid')).innerHTML = "";
                document.getElementById(input.getAttribute('data-validationtextid')).innerHTML = "Please enter a valid email address!";
            }
        } else if (input.type === "password") {
            let password = input.value.trim();
            if (password.length >= 8) {
                if (password.match("^(?=.*?[0-9]).{8,}$")) {
                    if (password.match("^(?=.*?[A-Z])(?=.*?[a-z]).{8,}$")) {
                        document.getElementById(input.getAttribute('data-validationtextid')).innerHTML = "";
                        input.classList.remove("notValidInput");

                    } else {
                        document.getElementById(input.getAttribute('data-validationtextid')).innerHTML = "";
                        document.getElementById(input.getAttribute('data-validationtextid')).innerHTML = "Your password has to contain capital and lowercase letters!";
                        input.classList.add("notValidInput");
                    }
                } else {
                    document.getElementById(input.getAttribute('data-validationtextid')).innerHTML = "";
                    document.getElementById(input.getAttribute('data-validationtextid')).innerHTML = "Your Password has to contain atleast one number!";
                    input.classList.add("notValidInput");
                }
            } else {
                document.getElementById(input.getAttribute('data-validationtextid')).innerHTML = "";
                document.getElementById(input.getAttribute('data-validationtextid')).innerHTML = "Your password has to be longer than 8 letters!";
                input.classList.add("notValidInput");
            }
        }
        else {
            if (input.value.length > 0) {
                document.getElementById(input.getAttribute('data-validationtextid')).innerHTML = "";
                input.classList.remove("emptyInput");
            } else {
                document.getElementById(input.getAttribute('data-validationtextid')).innerHTML = "";
                document.getElementById(input.getAttribute('data-validationtextid')).innerHTML = "You have to fill in this field!";
                input.classList.add("emptyInput");
            }
        }
    }

    //when textarea required => check if string.length is bigger then 0
    function validateTextarea(textarea) {
        if (textarea.hasAttribute("required")) {
            if (textarea.value.length > 0) {
                textarea.classList.remove("emptyTextarea");
                document.getElementById(textarea.getAttribute('data-validationtextid')).innerHTML = "";
            } else {
                textarea.classList.add("emptyTextarea");
                document.getElementById(textarea.getAttribute('data-validationtextid')).innerHTML = "";
                document.getElementById(textarea.getAttribute('data-validationtextid')).innerHTML = "You have to fill in this field!";
            }
        }
    }

    //returns if all inputs in form are valid
    function allInputsValid(form) {
        let allInputsValid = true;
        for (let input of inp) {
            let form = input.form;

            if (input.hasAttribute("required")) {
                if (input.type === "email") {
                    if (!validateEmail(input.value)) {
                        allInputsValid = false;
                    }
                } else if (input.type === "password") {
                    let password = input.value.trim();
                    if (input.value.length < 8) {
                        allInputsValid = false;
                        if (!password.match("^(?=.*?[0-9]).{8,}$")) {
                            allInputsValid = false;
                            if (!password.match("^(?=.*?[A-Z])(?=.*?[a-z]).{8,}$")) {
                                allInputsValid = false;
                            }
                        }
                    }
                }
            }
        }
        return allInputsValid;
    }
}