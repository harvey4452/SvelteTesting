<script>
    export let CustomerEmail;
    let API_URL = "http://localhost:5035/";

    let exerciseNames;
    let exerciseReps;
    let data;
    let displayChart = 'false';

    let exercises = [
		{ text: `Bench press` },
		{ text: `Leg press` },
		{ text: `Leg curl` },
        { text: `Leg extensions` },
		{ text: `Chest dips` },
		{ text: `Chest flies` }
	];

	let selectedExercise;

    function AssignData()
    {
        data = {
        labels: exerciseNames,
        datasets: [
        {
            label: 'Reps',
            data: exerciseReps,
            backgroundColor: [
            '#F7464A',
            '#46BFBD',
            '#FDB45C',
            '#949FB1',
            '#4D5360',
            '#AC64AD',
            ],
            hoverBackgroundColor: [
            '#FF5A5E',
            '#5AD3D1',
            '#FFC870',
            '#A8B3C5',
            '#616774',
            '#DA92DB',
            ],
        },
        ],
    };
        displayChart = 'true';
    }

    function GetReps()
    {
        let newUrl = API_URL+"api/GymApp/GetReps?pEmail="+CustomerEmail;
        fetch(newUrl)
        .then((response)=>response.json())
        .then((data)=>{
            const reps = JSON.parse(data);
            exerciseReps = reps;
            GetExercises();
        });
    }
    function GetExercises()
    {
        let newUrl = API_URL+"api/GymApp/GetExerciseNames?pEmail="+CustomerEmail;
        fetch(newUrl)
        .then((response)=>response.json())
        .then((data)=>{
            const exercises = JSON.parse(data);
            exerciseNames = exercises;
            AssignData();
        });
    }

    function IncrementRep()
    {
        let newUrl = API_URL+"api/GymApp/AddReps?pEmail="+CustomerEmail+"&pExercise="+selectedExercise.text+"&pReps=1";
        fetch(newUrl,{method:"POST"})
		.then((response)=>response.json())
		.then((data)=>{
            GetReps();
		});
    }

	import { onMount } from 'svelte';

	onMount(() => {
        GetReps();
	});

    import { Pie } from 'svelte-chartjs';
    import {
        Chart as ChartJS,
        Title,
        Tooltip,
        Legend,
        ArcElement,
        CategoryScale,
  } from 'chart.js';

  ChartJS.register(Title, Tooltip, Legend, ArcElement, CategoryScale);
</script>

<div class="row">
    <div class="column">
        <div class="pieChartContainer self-align-center">
            {#if displayChart == 'true'}
                <Pie {data} options={{ responsive: true }} />
            {/if}
        </div>
    </div>
    <div class="column">
        <div class="self-align-center">
            <select bind:value={selectedExercise}>
                {#each exercises as exercise}
                    <option value={exercise}>
                        {exercise.text}
                    </option>
                {/each}
	        </select>
        </div>
    </div>
    <div class="align-self-center">
        <button class="buttonStyle" on:click={IncrementRep}>Add rep</button>
    </div>
</div>

<style>
    .pieChartContainer{
        max-height: 100%;
        max-width: 100%;
    }

    .column{
        float: left;
        width: 25%;
    }
</style>