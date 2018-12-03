const problemsTableId = 'problems-table';
const sortByNameButtonId = 'sort-by-name-button';
const sortByGradeButtonId = 'sort-by-grade-button';

populateTableSortByName(problemsTableId);

document.getElementById(sortByNameButtonId).onclick = function () {
    document.getElementById(sortByNameButtonId).classList.add('selectedButton');
    document.getElementById(sortByGradeButtonId).classList.remove('selectedButton');

    populateTableSortByName(problemsTableId);
};

document.getElementById(sortByGradeButtonId).onclick = function () {
    document.getElementById(sortByNameButtonId).classList.remove('selectedButton');
    document.getElementById(sortByGradeButtonId).classList.add('selectedButton');

    populateTableSortByGrade(problemsTableId);
};

document.getElementById('search-by-holds-button').onclick = function () {
    const holdsEntered = document.getElementById('search-by-holds-input').value.split(' ');
    populateTableByHoldsContained(holdsEntered, problemsTableId, function () {
        alert('Invalid hold(s) entered.');
    });
};