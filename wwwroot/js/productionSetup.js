
    document.getElementById("stateDropdown").addEventListener("change", function () {
        this.form.submit();
        });

    document.addEventListener("DOMContentLoaded", function () {
            const calendarEl = document.getElementById("calendar");
    const calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'timeGridWeek',
    slotMinTime: '06:00:00',
    slotMaxTime: '22:00:00',
    height: 600,
    locale: 'en',
    firstDay: 1,
    events: []
            });

        fetch('/Calendar/GetProductionWorkSessions?productionId=@Model.ProductionId')
                .then(response => response.json())
                .then(data => {
        calendar.addEventSource(data);
    calendar.render();
                })
                .catch(error => console.error("Error loading work sessions:", error));
        });

