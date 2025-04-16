document.addEventListener("DOMContentLoaded", function () {
    const teamId = document.getElementById("teamSelect").value;

    if (teamId) {
        loadTeamTasks(teamId);
        loadCalendar(teamId);
    }

    document.getElementById("teamSelect").addEventListener("change", function () {
        const selectedTeamId = this.value;
        if (selectedTeamId) {
            loadTeamTasks(selectedTeamId);
            loadCalendar(selectedTeamId);
        } else {
            document.getElementById("taskList").innerHTML = "<li>Select team for list of planned tasks.</li>";
            document.getElementById("calendar").innerHTML = "";
        }
    });

    // Automatyczne uzupełnianie planowanego zakończenia
    document.getElementById("startTime").addEventListener("change", function () {
        const start = new Date(this.value);
        const duration = parseInt(document.getElementById("EstimatedDurationMinutes").value);

        if (!isNaN(start.getTime()) && !isNaN(duration)) {
            const end = new Date(start.getTime() + duration * 60000);

            const year = end.getFullYear();
            const month = String(end.getMonth() + 1).padStart(2, '0');
            const day = String(end.getDate()).padStart(2, '0');
            const hours = String(end.getHours()).padStart(2, '0');
            const minutes = String(end.getMinutes()).padStart(2, '0');

            const localEnd = `${year}-${month}-${day}T${hours}:${minutes}`;
            document.getElementById("endTime").value = localEnd;
        }
    });
});

function loadTeamTasks(teamId) {
    fetch(`/Production/GetTeamTasks?teamId=${teamId}`)
        .then(response => response.json())
        .then(data => {
            const taskList = document.getElementById("taskList");
            taskList.innerHTML = "";

            if (data.length > 0) {
                data.forEach(task => {
                    const plannedStartTime = new Date(task.plannedStartTime);
                    const plannedEndTime = new Date(task.plannedEndTime);

                    if (isNaN(plannedStartTime) || isNaN(plannedEndTime)) {
                        return;
                    }

                    const li = document.createElement("li");
                    li.textContent = `${task.taskName} - ${plannedStartTime.toLocaleString()} do ${plannedEndTime.toLocaleString()}`;
                    taskList.appendChild(li);
                });
            } else {
                taskList.innerHTML = "<li>No planned tasks for this team.</li>";
            }
        })
        .catch(error => {
            console.error("Error downloading data:", error);
        });
}

function loadCalendar(teamId) {
    fetch(`/Production/GetTeamTasksForCalendar?teamId=${teamId}`)
        .then(response => response.json())
        .then(data => {
            const calendarEl = document.getElementById("calendar");

            // Czyść poprzedni kalendarz
            calendarEl.innerHTML = "";

            const calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'timeGridWeek',
                slotMinTime: '06:00:00',
                slotMaxTime: '22:00:00',
                height: 500,
                locale: 'en',
                firstDay: 1,
                events: data.map(t => ({
                    title: t.title,
                    start: t.start,
                    end: t.end
                }))
            });

            calendar.render();
        })
        .catch(error => {
            console.error("Error loading calendar:", error);
        });
}