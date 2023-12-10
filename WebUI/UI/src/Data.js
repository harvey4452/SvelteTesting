export const data = {
    labels: ['Red', 'Green', 'Yellow', 'Grey', 'Dark Grey'],
    datasets: [
      {
        data: [300, 50, 100, 40, 120],
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

  function GetReps(){
    let newUrl = API_URL+"api/GymApp/GetReps?pEmail="+DeleteEmail;
    fetch(newUrl,{method:"DELETE"})
    .then((response)=>response.json())
    .then((data)=>{
      if (data == "Account deleted!")
      {
        displayDeleteMessage = 'success';
      }
      else if(data == "No account with that email exists!")
      {
        displayDeleteMessage = 'error';
      }
    });
  }