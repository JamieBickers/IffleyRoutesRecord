const problemsTableId = 'problems-table';
const sortByNameButtonId = 'sort-by-name-button';
const sortByGradeButtonId = 'sort-by-grade-button';

populateTableSortByName(problemsTableId);

document.getElementById(sortByNameButtonId).onclick = function () {
    document.getElementById(sortByNameButtonId).classList.add('selectedButton');
    document.getElementById(sortByGradeButtonId).classList.remove('selectedButton');

    populateTableSortByName(problemsTableId);
};

document.getElementById('sort-by-grade-button').onclick = function () {
    document.getElementById(sortByNameButtonId).classList.remove('selectedButton');
    document.getElementById(sortByGradeButtonId).classList.add('selectedButton');

    populateTableSortByGrade(problemsTableId);
};