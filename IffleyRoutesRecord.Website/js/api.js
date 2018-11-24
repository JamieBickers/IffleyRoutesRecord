//const baseUrl = 'http://localhost:61469/api';
const baseUrl = 'https://iffley-routes-record.herokuapp.com/api';

function getProblems(callBack) {
    fetch(`${baseUrl}/problem`)
        .then(function (response) {
            return response.json();
        })
        .then(function (json) {
            callBack(json);
        });
}