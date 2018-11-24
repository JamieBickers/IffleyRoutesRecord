function populateTableSortByName(tableId) {
    populateTable(function (a, b) {
        if (a.name < b.name) {
            return -1;
        }
        if (a.name > b.name) {
            return 1;
        }
        return 0;
    }, tableId);
}

function populateTableSortByGrade(tableId) {
    populateTable(function (a, b) {
        return a.globalGrade - b.globalGrade;
    }, tableId);
}

function populateTable(sorter, tableId) {
    document.getElementById(tableId).getElementsByTagName('tbody')[0].innerHTML = '';

    getProblems(function (problems) {
        problems = problems.sort(sorter);
        for (let i = 0; i < problems.length; i++) {
            addProblemToTable(problems[i], tableId);
        }
    });
}

function addProblemToTable(problem, tableId) {
    let table = document.getElementById(tableId).getElementsByTagName('tbody')[0];

    let row = table.insertRow();
    const rowClass = getGradeClass(problem);
    row.classList.add(rowClass);

    let cell1 = row.insertCell(0);
    let cell2 = row.insertCell(1);
    let cell3 = row.insertCell(2);
    let cell4 = row.insertCell(3);

    cell1.innerHTML = problem.name;
    cell2.innerHTML = formatGrades(problem);
    cell3.innerHTML = formatHolds(problem.holds);
    cell4.innerHTML = formatProblemRules(problem.rules);
}

function formatHolds(holds) {
    let formatted = '';

    for (let i = 0; i < holds.length; i++) {
        const hold = holds[i];
        formatted += `${hold.name} `;

        if (hold.holdRules !== null && hold.holdRules !== undefined && hold.holdRules.length > 0) {
            formatted += '(';

            formatted += hold.holdRules
                .map(function (rule) {
                    return rule.name;
                })
                .join(", ");

            formatted += ') ';
        }
    }

    return formatted;
}

function formatProblemRules(rules) {
    return rules
        .map(function (rule) {
            return rule.name;
        })
        .join(", ");
}

function formatGrades(problem) {
    return [problem.bGrade, problem.techGrade, problem.poveyGrade, problem.furlongGrade]
        .filter(function (elt) {
            return elt !== null;
        })
        .map(function (elt) {
            return elt.name;
        })
        .join(' ');
}

function getGradeClass(problem) {
    if (problem.globalGrade <= 9) {
        return 'easyProblem';
    }
    else if (problem.globalGrade <= 15) {
        return 'mediumProblem';
    }
    else if (problem.globalGrade <= 18) {
        return 'thresholdProblem';
    }
    else {
        return 'hardProblem';
    }
}