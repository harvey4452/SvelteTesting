<script>
	let name = 'world';

	let email;
	let password;

	import ImportedHeader from './header.svelte';

	let API_URL = "http://localhost:5035/";
	let isValid;

	function GetIsValid(){
		let newUrl = API_URL+"api/GymApp/GetCredentials?pEmail="+email+"&pPassword="+password;
		alert(newUrl);
		fetch(newUrl)
		.then((response)=>response.json())
		.then((data)=>{
			alert(data);
			isValid=data;
		});
	}

</script>

<div>
	<ImportedHeader />
	<button on:click={GetIsValid}>Verify credentials</button>
	<input bind:value={email} placeholder="enter your email" />
	<input bind:value={password} placeholder="enter your password" />
	<p>{isValid}</p>
</div>



<style>

	p{
		text-color:blue;
		color:blue;
	}

</style>
