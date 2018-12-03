const alphabeticCharacters = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';

function populateTableSortByName(tableId) {
    populateTable(function (a, b) {
        const aName = onlyAlphabeticCharactersAndToLowerCase(a.name);
        const bName = onlyAlphabeticCharactersAndToLowerCase(b.name);

        if (aName < bName) {
            return -1;
        }
        if (aName > bName) {
            return 1;
        }
        return 0;
    }, tableId);
}

function onlyAlphabeticCharactersAndToLowerCase(str) {
    return str
        .toLowerCase()
        .split('')
        .filter(function (chr) {
            return alphabeticCharacters.indexOf(chr) > -1;
        })
        .join('');
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

    cell1.classList.add("cold-md-3", "col-sm-3");
    cell2.classList.add("cold-md-3", "col-sm-3");
    cell3.classList.add("cold-md-3", "col-sm-3");
    cell4.classList.add("cold-md-3", "col-sm-3");

    let reportIssueIcon = document.createElement('i');
    reportIssueIcon.classList.add('material-icons', 'standard-user');
    reportIssueIcon.style = 'font-size:18px;color:red;cursor:pointer;display:none;';
    reportIssueIcon.innerHTML = 'error';

    reportIssueIcon.onclick = () => reportIssue(problem);

    cell1.appendChild(reportIssueIcon);

    let problemNameSpan = document.createElement('span');
    problemNameSpan.innerHTML = problem.name;
    cell1.appendChild(problemNameSpan);

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

function reportIssue(problem) {
    createProblemIssue({
        problemId: problem.problemId,
        description: prompt(`Describe the issue with ${problem.name}`)
    }, function (response) {
        console.log(response);
    });
}

function populateTableByHoldsContained(holdNames, tableId, callbackIfInvalidHold) {
    getProblems(function (problems) {
        getHolds(function (allHolds) {
            const holds = holdNames.map(holdName => allHolds.find(hold => hold.name === holdName));
            if (holds.some(hold => hold === undefined)) {
                callbackIfInvalidHold();
            }
            else {
                const sortedProblems = sortProblemsByHoldsContained(problems, holds, allHolds);
                document.getElementById(tableId).getElementsByTagName('tbody')[0].innerHTML = '';

                for (let i = 0; i < sortedProblems.length; i++) {
                    addProblemToTable(sortedProblems[i], tableId);
                }
            }
        });
    });
}

function sortProblemsByHoldsContained(problems, holds, allHolds) {
    problems.forEach(problem => {
        problem.numberOfHoldsInCommon = getNumberOfHoldsInCommonWithProblem(problem, holds, allHolds);
    });

    return problems.sort((a, b) => b.numberOfHoldsInCommon - a.numberOfHoldsInCommon);
}

function getNumberOfHoldsInCommonWithProblem(problem, holds, allHolds) {
    let numberOfHoldsInCommon = 0;

    for (let i = 0; i < problem.holds.length; i++) {
        const problemHold = problem.holds[i];
        if (holds.find(hold => hold.holdId === problemHold.holdId || hold.holdId === problemHold.parentHoldId)) {
            numberOfHoldsInCommon++;
        }
    }
    return numberOfHoldsInCommon;
}