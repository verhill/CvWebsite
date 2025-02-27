// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//Visar Bekräfta och avbryt knapparna när man vill ta bort meddelande döljer dem sen när man klickar avbryt
function showConfirmation(button) {
    button.style.display = 'none';
    button.nextElementSibling.style.display = 'block';
}

function hideConfirmation(button) {
    var confirmationButtons = button.parentElement;
    var deleteButton = confirmationButtons.previousElementSibling;
    confirmationButtons.style.display = 'none';
    deleteButton.style.display = 'block';
}

// Alla funktioner fungerar likadant fast uppdelat i på varje ID i vår html och ID från varje objekt, hämtar sedan objekt.Name (kolla vår li)
function SearchSkill(Id, isChecked) {
    const selectedSkillsList = document.getElementById("NS");
    const skillName = document.querySelector(`.f-i1 input[value='${Id}']`).parentElement.textContent.trim();

    //Kollar checkboxen
    if (isChecked) {
        const newListItem = document.createElement("li");
        newListItem.textContent = skillName;
        newListItem.id = `SS2-${Id}`;
        selectedSkillsList.appendChild(newListItem);
    } else {

        const itemToRemove = document.getElementById(`SS2-${Id}`);
        if (itemToRemove) {
            selectedSkillsList.removeChild(itemToRemove);
        }
    }
}

function SearchE(Id, isChecked) {
    const selectedEducationList = document.getElementById("NED");
    const eName = document.querySelector(`.f-i2 input[value='${Id}']`).parentElement.textContent.trim();


    if (isChecked) {
        const newListItem = document.createElement("li");
        newListItem.textContent = eName;
        newListItem.id = `SED2-${Id}`;
        selectedEducationList.appendChild(newListItem);
    } else {

        const itemToRemove = document.getElementById(`SED2-${Id}`);
        if (itemToRemove) {
            selectedEducationList.removeChild(itemToRemove);
        }
    }
}
function SearchEx(Id, isChecked) {
    const selectedExList = document.getElementById("NE");
    const experienceName = document.querySelector(`.f-i3 input[value='${Id}']`).parentElement.textContent.trim();


    if (isChecked) {
        const newListItem = document.createElement("li");
        newListItem.textContent = experienceName;
        newListItem.id = `SE2-${Id}`;
        selectedExList.appendChild(newListItem);
    } else {

        const itemToRemove = document.getElementById(`SE2-${Id}`);
        if (itemToRemove) {
            selectedExList.removeChild(itemToRemove);
        }
    }
}