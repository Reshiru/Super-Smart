window.onload = () => {

    const mailInput = document.getElementById("mailInput");
    const passwordInput = document.getElementById("passwordInput");
    mailInput.style.outline = "none";





    let inp = document.getElementsByTagName('input');

    for (let input of inp) {
        input.id = guid();

        $("#" + input.id).after("<span id='errorMessage-"+input.id+"' class='errormessage'></span>");
    }


    for (let input of inp) {

        if (input.type === "email") {
            console.log("found email");
            input.onkeyup = (e) => {
                    if (validateEmail(input.value)) {
                        input.classList.remove("wrongEmail");

                        input.classList.add("validateEmail");
                        $("#errorMessage-"+input.id).html("");



                    } else {
                        input.classList.remove("validateEmail");
                        input.classList.add("wrongEmail");
                        $("#errorMessage-" + input.id).html("");
                        $("#errorMessage-" + input.id).html("Please enter a valid email address");
                    }
         
          }
        }
    }



    function guid() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    }

    function validateEmail(email) {
        const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(email).toLowerCase());
    }
}