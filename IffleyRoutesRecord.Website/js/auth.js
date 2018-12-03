window.addEventListener('load', function () {
    const webAuth = new auth0.WebAuth({
        domain: 'iffley-bouldering.eu.auth0.com',
        clientID: '7cqQ4JuBUoL2nNPrhC2RbNZIoa3hcvHT',
        responseType: 'token id_token',
        scope: 'openid profile email update:problem create:problem create:issue read:issue',
        redirectUri: window.location.href,
        audience: 'https://iffley-routes-record.herokuapp.com'
    });

    let loginBtn = document.getElementById('btn-login');
    let logoutBtn = document.getElementById('btn-logout');
    let emailDisplay = document.getElementById('email-display');

    handleAuthentication();

    loginBtn.onclick = function (e) {
        e.preventDefault();
        webAuth.authorize();
    };

    logoutBtn.onclick = logout;

    function handleAuthentication() {
        webAuth.parseHash(function (err, authResult) {
            if (authResult && authResult.accessToken && authResult.idToken) {
                window.location.hash = '';
                setSession(authResult);
                handleUserAccountDisplay(authResult.idTokenPayload.email);
            } else if (err) {
                alert(
                    'Error: ' + err.error + '. Check the console for further details.'
                );
            }
            else {
                handleUserAccountDisplay('');
            }
        });
    }

    function setSession(authResult) {
        // Set the time that the Access Token will expire at
        const expiresAt = JSON.stringify(
            authResult.expiresIn * 1000 + new Date().getTime()
        );
        localStorage.setItem('access_token', authResult.accessToken);
        localStorage.setItem('id_token', authResult.idToken);
        localStorage.setItem('expires_at', expiresAt);
    }

    function logout() {
        // Remove tokens and expiry time from localStorage
        localStorage.removeItem('access_token');
        localStorage.removeItem('id_token');
        localStorage.removeItem('expires_at');
        handleUserAccountDisplay('');
    }

    function isAuthenticated() {
        // Check whether the current time is past the
        // Access Token's expiry time
        const expiresAt = JSON.parse(localStorage.getItem('expires_at'));
        return new Date().getTime() < expiresAt;
    }

    function handleUserAccountDisplay(email) {
        let standardUserControls = document.getElementsByClassName('standard-user');
        if (isAuthenticated()) {
            loginBtn.style.display = 'none';
            logoutBtn.style.display = 'inline-block';

            emailDisplay.innerHTML = `Logged in as: ${email}`;

            for (let i = 0; i < standardUserControls.length; i++) {
                standardUserControls[i].style.display = 'inline-block';
            }
        } else {
            loginBtn.style.display = 'inline-block';
            logoutBtn.style.display = 'none';

            emailDisplay.innerHTML = 'Not logged in';

            for (let i = 0; i < standardUserControls.length; i++) {
                standardUserControls[i].style.display = 'none';
            }
        }
    }
});