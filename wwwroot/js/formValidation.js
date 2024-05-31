document.addEventListener('DOMContentLoaded', function () {
    const signupForm = document.getElementById('signupForm');
    const signinForm = document.getElementById('signinForm');

    if (signupForm) {
        signupForm.addEventListener('submit', function (event) {
            let valid = true;

            const firstName = document.getElementById('FirstName');
            const lastName = document.getElementById('LastName');
            const email = document.getElementById('Email');
            const password = document.getElementById('Password');
            const confirmPassword = document.getElementById('ConfirmPassword');
            const terms = document.getElementById('TermsAndConditions');

            if (firstName.value.trim() === '') {
                showError(firstName, 'First Name Needed');
                valid = false;
            } else {
                clearError(firstName);
            }

            if (lastName.value.trim() === '') {
                showError(lastName, 'Last Name Needed');
                valid = false;
            } else {
                clearError(lastName);
            }

            if (email.value.trim() === '') {
                showError(email, 'Email Needed');
                valid = false;
            } else if (!validateEmail(email.value)) {
                showError(email, 'Invalid Email');
                valid = false;
            } else {
                clearError(email);
            }

            if (password.value.trim() === '') {
                showError(password, 'Password Needed');
                valid = false;
            } else if (password.value.length < 6) {
                showError(password, 'You have to enter minimum of 6 characters or digits');
                valid = false;
            } else {
                clearError(password);
            }

            if (confirmPassword.value.trim() === '') {
                showError(confirmPassword, 'Confirm password Needed');
                valid = false;
            } else if (password.value !== confirmPassword.value) {
                showError(confirmPassword, 'Password does not match');
                valid = false;
            } else {
                clearError(confirmPassword);
            }

            if (!terms.checked) {
                showError(terms, 'You have to accept the Terms');
                valid = false;
            } else {
                clearError(terms);
            }

            if (!valid) {
                event.preventDefault();
            }
        });
    }

    if (signinForm) {
        signinForm.addEventListener('submit', function (event) {
            let valid = true;

            const email = document.getElementById('SignInEmail');
            const password = document.getElementById('SignInPassword');

            if (email.value.trim() === '') {
                showError(email, 'Email needed');
                valid = false;
            } else if (!validateEmail(email.value)) {
                showError(email, 'Invalid Email');
                valid = false;
            } else {
                clearError(email);
            }

            if (password.value.trim() === '') {
                showError(password, 'Password needed');
                valid = false;
            } else {
                clearError(password);
            }

            if (!valid) {
                event.preventDefault();
            }
        });
    }

    function showError(input, message) {
        const errorElement = input.nextElementSibling;
        errorElement.textContent = message;
        errorElement.style.display = 'block';
    }

    function clearError(input) {
        const errorElement = input.nextElementSibling;
        errorElement.textContent = '';
        errorElement.style.display = 'none';
    }

    function validateEmail(email) {
        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(String(email).toLowerCase());
    }
});
