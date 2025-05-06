document.addEventListener("DOMContentLoaded", function () {
    const teamSelect = document.getElementById("teamSelect");
    const calendarContainer = document.getElementById("calendarContainer");

    let initialTeamId = null;

    if (teamSelect && teamSelect.value) {
        initialTeamId = teamSelect.value;
    } else if (calendarContainer && calendarContainer.dataset.teamId) {
        initialTeamId = calendarContainer.dataset.teamId;
    }

    if (initialTeamId) {
        loadTeamTasks(initialTeamId);
        loadCalendar(initialTeamId);
    }

    if (teamSelect) {
        teamSelect.addEventListener("change", function () {
            const selectedTeamId = this.value;
            if (selectedTeamId) {
                loadTeamTasks(selectedTeamId);
                loadCalendar(selectedTeamId);
            } else {
                const taskList = document.getElementById("taskList");
                if (taskList) taskList.innerHTML = "<li>Select team for list of planned tasks.</li>";
                document.getElementById("calendar").innerHTML = "";
            }
        });
    }

    const startTimeInput = document.getElementById("startTime");
    const durationInput = document.getElementById("EstimatedDurationMinutes");
    const endTimeInput = document.getElementById("endTime");

    if (startTimeInput && durationInput && endTimeInput) {
        startTimeInput.addEventListener("change", function () {
            const start = new Date(this.value);
            const duration = parseInt(durationInput.value);

            if (!isNaN(start.getTime()) && !isNaN(duration)) {
                const end = new Date(start.getTime() + duration * 60000);

                const year = end.getFullYear();
                const month = String(end.getMonth() + 1).padStart(2, '0');
                const day = String(end.getDate()).padStart(2, '0');
                const hours = String(end.getHours()).padStart(2, '0');
                const minutes = String(end.getMinutes()).padStart(2, '0');

                const localEnd = `${year}-${month}-${day}T${hours}:${minutes}`;
                endTimeInput.value = localEnd;
            }
        });
    }
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
            calendarEl.innerHTML = ""; // czyść poprzedni kalendarz

            const calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'timeGridWeek',
                slotMinTime: '06:00:00',
                slotMaxTime: '22:00:00',
                height: 500,
                locale: 'en',
                firstDay: 1,
                events: data.map(t => ({
                    id: t.id,       // potrzebne do kliknięcia
                    title: t.title,
                    start: t.start,
                    end: t.end,
                    color: t.isCompleted ? 'green' : '',
                })),
                eventClick: function (info) {
                    const taskId = info.event.id;
                    if (taskId) {
                        window.location.href = `/Workplace/${taskId}`;
                    }
                }
            });

            calendar.render();
        })
        .catch(error => {
            console.error("Error loading calendar:", error);
        });
}