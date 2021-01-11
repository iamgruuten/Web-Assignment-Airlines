const FloatLabel = (() => {
    // add active class and placeholder
    const handleFocus = (e) => {
        const target = e.target;
        target.parentNode.classList.add('active');
        target.setAttribute('placeholder', target.getAttribute('data-placeholder'));
    };

    // remove active class and placeholder
    const handleBlur = (e) => {
        const target = e.target;
        if (!target.value) {
            target.parentNode.classList.remove('active');
        }
        target.removeAttribute('placeholder');
    };

    // register events
    const bindEvents = (element) => {
        const floatField = element.querySelector('input');
        floatField.addEventListener('focus', handleFocus);
        floatField.addEventListener('blur', handleBlur);
    };

    // get DOM elements
    const init = () => {
        const floatContainers = document.querySelectorAll('.float-container');

        floatContainers.forEach((element) => {
            if (element.querySelector('input').value) {
                element.classList.add('active');
            }

            bindEvents(element);
        });
    };

    return {
        init: init
    };
})();

FloatLabel.init();

function addMonths(date, months) {
    var d = date.getDate();
    date.setMonth(date.getMonth() + +months);
    if (date.getDate() != d) {
        date.setDate(0);
    }
    return date;
}

Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}

function formatDateMinMax(date) {
    var dd = date.getDate();
    var mm = date.getMonth() + 1;
    var yyyy = date.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }

    date = yyyy + '-' + mm + '-' + dd;

    return date;
}

//Next Section - Set MinDate and MaxDate
var today = new Date().addDays(1);
var MaxDate = addMonths(today, 8);

var todaydate = new Date().addDays(1);

todaydate = formatDateMinMax(todaydate);
MaxDate = formatDateMinMax(MaxDate);

document.getElementById("departDateID").setAttribute("min", todaydate);
document.getElementById("departDateID").setAttribute("max", MaxDate);
document.getElementById("returnDateID").setAttribute("min", todaydate);
document.getElementById("returnDateID").setAttribute("max", MaxDate);

//Todo if onclick, the date min && max should auto change
function departDateOnChange() {
    var returnNewMinDate = document.getElementById("departDateID").value;
    var getReturnValue = document.getElementById("returnDateID").value;

    document.getElementById("returnDateID").setAttribute("min", returnNewMinDate);

    if (returnNewMinDate > getReturnValue) {
        document.getElementById("returnDateID").valueAsDate = null;
    }
}

//TODO set default password checker
var currentValue = 0;
function handleClick(myRadio) {
    currentValue = myRadio.value;

    if (currentValue == "Yes") {
        document.getElementById("Password").readOnly = true;
        document.getElementById("Password").value = "p@55Cust";

        document.getElementById("confirmPassword").readOnly = true;
        document.getElementById("confirmPassword").value = "p@55Cust";
    } else {
        document.getElementById("Password").disabled = false;
        document.getElementById("Password").value = "";
        document.getElementById("confirmPassword").disabled = false;
        document.getElementById("confirmPassword").value = "";
    }
}