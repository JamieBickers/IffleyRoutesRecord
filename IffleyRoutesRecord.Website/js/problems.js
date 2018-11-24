function populateTable() {
    getProblems(function (problems) {
        for (var i = 0; i < problems.length; i++) {
            addProblemToTable(problems[i], 'problems-table');
        }
    });
}

function addProblemToTable(problem, tableId) {
    let table = document.getElementById(tableId);

    var row = table.insertRow();

    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    var cell3 = row.insertCell(2);
    var cell4 = row.insertCell(3);
    var cell5 = row.insertCell(4);
    var cell6 = row.insertCell(5);

    cell1.innerHTML = problem.name;
    cell2.innerHTML = problem.techGrade === null ? '' : problem.techGrade.name;
    cell3.innerHTML = problem.bGrade === null ? '' : problem.bGrade.name;
    cell4.innerHTML = problem.poveyGrade === null ? '' : problem.poveyGrade.name;
    cell5.innerHTML = problem.furlongGrade === null ? '' : problem.furlongGrade.name;
    cell6.innerHTML = formatHolds(problem.holds);
}

function formatHolds(holds) {
    return holds
        .map(hold => hold.name)
        .join(" ");
}

populateTable();