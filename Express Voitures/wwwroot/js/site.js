document.addEventListener("DOMContentLoaded", function () {
    // Fonction pour afficher ou masquer la date de vente en fonction du statut sélectionné
    function toggleSaleDate() {
        var statusSelect = document.getElementById('statusSelect');
        var saleDateGroup = document.getElementById('saleDateGroup');
        if (statusSelect.value === 'Vendu') {
            saleDateGroup.style.display = 'block';
        } else {
            saleDateGroup.style.display = 'none';
        }
    }

    // Ajouter un événement de changement pour le sélecteur de statut, et initialiser la visibilité de la date de vente au chargement de la page
    if (document.getElementById('statusSelect')) {
        document.getElementById('statusSelect').addEventListener('change', toggleSaleDate);
        toggleSaleDate(); // Initialize the visibility on page load
    }

    // Ajouter un événement de clic pour le bouton d'ajout de réparation
    if (document.getElementById('add-repair')) {
        document.getElementById('add-repair').addEventListener('click', function () {
            var description = document.getElementById('new-repair-description').value;
            var cost = document.getElementById('new-repair-cost').value;

            if (description && cost) {
                var repairItem = document.createElement('tr');
                repairItem.innerHTML = `<td>${description}<input type="hidden" name="RepairsDescriptions" value="${description}" /></td>
                                        <td>${cost} €<input type="hidden" name="RepairsCosts" value="${cost}" /></td>
                                        <td><button type="button" class="btn btn-danger btn-sm remove-repair">Supprimer</button></td>`;
                document.getElementById('repairs-list').appendChild(repairItem);

                // Réinitialiser les champs de saisie
                document.getElementById('new-repair-description').value = '';
                document.getElementById('new-repair-cost').value = '';
            }
        });
    }

    // Ajouter un événement de clic pour supprimer une réparation
    document.addEventListener('click', function (e) {
        if (e.target && e.target.classList.contains('remove-repair')) {
            e.target.closest('tr').remove();
        }
    });

    // Prévisualiser une nouvelle photo lorsque l'utilisateur sélectionne un fichier
    if (document.getElementById('photoInput')) {
        document.getElementById('photoInput').addEventListener('change', function (e) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var preview = document.getElementById('newPhotoPreview');
                preview.src = e.target.result;
                preview.style.display = 'block';
            }
            reader.readAsDataURL(this.files[0]);

            var fileNameSpan = document.getElementById('fileName');
            fileNameSpan.textContent = this.files[0].name;
        });
    }

    // Ajouter un champ caché pour le statut de la voiture lors de la soumission du formulaire d'édition
    if (document.getElementById('editForm')) {
        document.getElementById('editForm').addEventListener('submit', function () {
            var statusElement = document.getElementById('statusSelect');
            var hiddenStatusInput = document.createElement('input');
            hiddenStatusInput.type = 'hidden';
            hiddenStatusInput.name = 'CarStatus.Status';
            hiddenStatusInput.value = statusElement.value;
            this.appendChild(hiddenStatusInput);

            if (statusElement.value !== 'Vendu') {
                var saleDateInput = document.querySelector('[name="SaleDate"]');
                if (saleDateInput) {
                    saleDateInput.value = '';
                }
            }
        });
    }

    // Corriger les entrées de prix pour remplacer les espaces et les virgules avant la soumission du formulaire
    const form = document.querySelector("form");
    if (form) {
        form.addEventListener("submit", function (event) {
            const priceInputs = form.querySelectorAll("input[data-type='currency']");
            priceInputs.forEach(function (input) {
                input.value = input.value.replace(/\s/g, '').replace(',', '.');
            });
        });
    }
});
