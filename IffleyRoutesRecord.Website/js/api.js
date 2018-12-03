const baseUrl = 'http://localhost:44319/api';
//const baseUrl = 'https://iffley-routes-record.herokuapp.com/api';

function getProblems(callBack) {
    fetch(`${baseUrl}/problem`)
        .then(function (response) {
            return response.json();
        })
        .then(function (json) {
            callBack(json);
        });
}

function createProblemIssue(createProblemIssueRequest, callBack) {
    console.log(localStorage.getItem('access_token'));
    fetch(`${baseUrl}/issue/problem`, {
        method: 'POST',
        body: JSON.stringify(createProblemIssueRequest),
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('access_token')}`
        }
    })
        .then(function (response) {
            return response.json();
        })
        .then(function (json) {
            callBack(json);
        });
}