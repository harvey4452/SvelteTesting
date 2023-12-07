<script>
    import CustomerHomepage from './CustomerHomepage.svelte';
    import AdminHomepage from './AdminHomepage.svelte';

    let email;
    let password;

    let API_URL = "http://localhost:5035/";

    let loggedIn = 'false';
    let displayErrorMessage = 'false';

	let isCustomer = 'false';
	let isAdmin = 'false';

	function VerifyCredentials(){
		let newUrl = API_URL+"api/GymApp/GetCredentials?pEmail="+email+"&pPassword="+password;
		fetch(newUrl)
		.then((response)=>response.json())
		.then((data)=>{
			if (data == "true"){
				loggedIn = "true";
				GetAccessLevel();
			}
			else{
				displayErrorMessage = "true";
			}
		});
	}
	function GetAccessLevel(){
		let newUrl = API_URL+"api/GymApp/GetAccessLevel?pEmail="+email;
		fetch(newUrl)
		.then((response)=>response.json())
		.then((data)=>{
			if (data == "Customer"){
				isCustomer = 'true';
			}
			else if (data == "Admin"){
				isAdmin = 'true';
			}
		});
	}

</script>

<div>
    {#if loggedIn == 'true'}
		{#if isCustomer == 'true'}
            <CustomerHomepage/>
        {:else if isAdmin == 'true'}
            <AdminHomepage/>
        {/if}
	{:else}
        {#if displayErrorMessage == 'true'}
            <p class="ErrorMessage">Incorrect email or password!</p>
        {/if}
		<input bind:value={email} placeholder="enter your email" />
	    <input bind:value={password} placeholder="enter your password" />
        <button on:click={VerifyCredentials}>Login</button>
	{/if}
</div>

<style>
	.ErrorMessage{
        color:red;
    }
</style>

