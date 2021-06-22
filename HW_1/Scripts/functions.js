//// https://api.themoviedb.org/3/search/tv?api_key=1e5a5ee20af326aebb685a34a1868b76&language=en-US&page=1&include_adult=false&query=Grey%27s%20Anatomy

$(document).ready(function () {
    $("#getTV").click(getTV);
    $("#formUser").submit(postUser);
    $("#loginform").submit(loginUser);
    $("#disconnect-btn").hide();
    $("#disconnect-btn").click(disconnectUser);

    getQuestions();

    //Randeer popular series by genre (35=comedy)
    getFavSeries(35);

    user = localStorage.getItem("user-login");
    if (user != null) {
        enterUser(user);
    }

    episodnum = 0;

    // replace it with your own key -DONE
    key = "1e5a5ee20af326aebb685a34a1868b76";
    img_tmdb = "../Images/img_tmdb.png";
    url = "https://api.themoviedb.org/";
    imagePath = "https://image.tmdb.org/t/p/w500";
});

//Get the most popular series by genre
function getFavSeries(genre) {
    //int genre = genre code
    //https://api.themoviedb.org/3/discover/tv?api_key=1e5a5ee20af326aebb685a34a1868b76&sort_by=popularity.desc&with_genres=35

    let apiSearch = `https://api.themoviedb.org/3/discover/tv?api_key=1e5a5ee20af326aebb685a34a1868b76&sort_by=popularity.desc&with_genres= ${genre}`;

    ajaxCall("GET", apiSearch, "", getSeriesPopByGenreSuccessCB, getSeriesPopByGenreErrorCB)

}

function getSeriesPopByGenreSuccessCB(seriesList) {
    console.log(seriesList);
}

function getSeriesPopByGenreErrorCB() {
    alert("Error");
}

function disconnectUser() {
    localStorage.clear();
    location.reload();

}

function postSeries(curr_tvshow) {
    let SeriesObj = {
        Id: curr_tvshow.id,
        Name: curr_tvshow.name,
        First_air_date: curr_tvshow.first_air_date,
        Origin_country: curr_tvshow.origin_country[0],
        Original_language: curr_tvshow.original_language,
        Overview: curr_tvshow.overview,
        Popularity: curr_tvshow.popularity,
        Poster_path: "https://image.tmdb.org/t/p/w500" + curr_tvshow.poster_path,
    };

    let api = "../api/Series";
    ajaxCall(
        "POST",
        api,
        JSON.stringify(SeriesObj),
        postSeriesSuccessCB,
        postSeriesErrorCB
    );
}

function postSeriesSuccessCB() {
    console.log("Series added")
    postEpisod(episodnum);
}

function postSeriesErrorCB() {
    console.log("Series error")

}

function userLoginToSystme(user) {
    localStorage.clear();
    delete user.Password;
    //console.log(JSON.stringify(user));
    localStorage.setItem("user-login", JSON.stringify(user));
    $("#register-btn").hide();
    $("#login-btn").hide();
    $("#disconnect-btn").show();

    $("#welcome-user").html(
        "<h6>Welcome " + user.Name + " " + user.LastName + "</h6>"
    );
}

function enterUser(user) {
    user = JSON.parse(user);
    $("#register-btn").hide();
    $("#login-btn").hide();
    $("#disconnect-btn").show();

    $("#welcome-user").html(
        "<h6>Welcome " + user.Name + " " + user.LastName + "</h6>"
    );
}

//../api/Users?mail=&password=..
function loginUser() {
    document.getElementById("id02").style.display = "none";
    let userlogin = $("#userlogin").val();
    console.log(userlogin);
    let passwordlogin = $("#passwordlogin").val();
    console.log(passwordlogin);
    api = "../api/Users?mail=" + userlogin + "&password=" + passwordlogin;
    console.log(api);
    ajaxCall("GET", api, "", loginUserSuccessCB, loginUserErrorCB);
    return false;
}

function loginUserErrorCB(e) {
    alert(e.responseJSON);
}

function loginUserSuccessCB(user) {
    console.log(user);
    if (user != null) {
        userLoginToSystme(user);
    } else if (user == null) {
        alert("Worng password or username");
    }
}

function postUser() {
    document.getElementById("id01").style.display = "none";

    let usrname = $("#usrname").val();
    let userlastname = $("#userlastname").val();
    let useremail = $("#useremail").val();
    let psw = $("#psw").val();
    let userphonenum = $("#userphonenum").val();
    let genderDDL = $("#genderDDL").val();
    let useryear = $("#useryear").val();
    let genreDDL = $("#genreDDL").val();
    let useraddress = $("#useraddress").val();
    if (genreDDL == "") {
        genreDDL == "Comedy";
    }
    let userObj = {
        Name: usrname,
        LastName: userlastname,
        Email: useremail,
        Password: psw,
        Cellphone: userphonenum,
        Gender: genderDDL,
        YearBirth: useryear,
        Genre: genreDDL,
        Address: useraddress,
        IsAdmin: 0,

    };

    //console.log(userObj)
    let api = "../api/Users";
    ajaxCall(
        "POST",
        api,
        JSON.stringify(userObj),
        postUserSuccessCB,
        postUserErrorCB
    );
    return false;
}

function postUserErrorCB() {
    alert("fail to add user");
}

function postUserSuccessCB() {
    alert("user added");
    //add to  localStorage
}

function getTV() {
    $(".container-tvshow").html("");
    let name = $("#tvShowName").val();
    let method = "3/search/tv?";
    api_key = "api_key=" + key;
    let moreParams = "&language=en-US&page=1&include_adult=false&";
    let query = "query=" + encodeURIComponent(name);
    let apiCall = url + method + api_key + moreParams + query;
    ajaxCall("GET", apiCall, "", getTVSuccessCB, getTVErrorCB);
}

function getTVSuccessCB(tv) {
    curr_tvshow = tv.results[0]
    tvId = tv.results[0].id;
    let poster = imagePath + tv.results[0].poster_path;
    str = "<img class='tv-show-img' src='" + poster + "'/>";
    $(".tv-show-image").html(str);//image of TV SHOW after search
    let method = "3/tv/";
    let api_key = "api_key=" + key;
    let apiCall = url + method + tvId + "?" + api_key; //^ change seasson 1 to multi

    ajaxCall("GET", apiCall, "", getSeasonSuccessCB, getSeasonErrorCB);
}

function renderSeason(season) {
    $(".season-render").html(
        '<select onchange="getEpisode(this.value);" name="Seasons" id="seasonselect"></select>'
    );
    //$("#seasonselect").append(`<option>Select</option>`)
    $(".tv-show-name").html(gSeason.name);
    $(".tv-show-overview").html(gSeason.overview);


    for (var i = 0; i < season.seasons.length; i++) {

        $("#seasonselect").append(
            "<option value=" +
            i +
            "|" +
            tvId +
            ">" +
            season.seasons[i].name +
            "</option>"
        );
    }
}

function getEpisode(value) {
    let temp = value.split("|");
    seasonNumber = temp[0];
    let tvID = temp[1];
    let method = "3/tv/";
    let method2 = "/season/";

    let apiCall =
        url +
        method +
        tvID +
        method2 +
        seasonNumber +
        "?" +
        api_key +
        "&language=en-US";

    ajaxCall("GET", apiCall, "", getEpisodeSuccessCB, getEpisodeErrorCB);
}

function getEpisodeSuccessCB(episod) {
    epi = episod;
    $(".render-episodes").html(`<div class="scrollbar" id="style-15">
            <div class="force-overflow">
            </div>
            </div>`);
    for (var i = 0; i < episod.episodes.length; i++) {
        x = JSON.stringify(curr_tvshow).split("'").join('')


        poster =
            "https://image.tmdb.org/t/p/w500" + episod.episodes[i].still_path;
        imgURL = "<img id='poster' src='" + poster + "'/>";
        $(".force-overflow").append(
            `<div class="episodecard"> ${imgURL} 
                        <h4 id="episod-name">${episod.episodes[i].name}</h4>
                        <h6 id="episod-date">${episod.episodes[i].air_date}</h6>
                        <button onclick='savenumber(${i});postSeries(${x})'; type='button' id="addtofav-btn" class='myButton'>Add to favorite</button>
                        

                    </div>`


            //"<div>Name: " +
            //episod.episodes[i].name +
            //"</div>" +
            //"<div>" +
            //imgURL +
            //"</div>" +
            //"<div>Overview: " +
            //episod.episodes[i].overview +
            //"</div>" +
            //"<div>Air Date: " +
            //episod.episodes[i].air_date +
            //"</div>" +
            //"<button onclick='savenumber(" + i + ");postSeries(" + x + ")'; type='button' class='btn btn-success'>Add Episode</button>" +
            //"<br>" +
            //"<br>"
        );
    }
}

function savenumber(i) {
    episodnum = i;
}

function postEpisod(i) {

    let episodeObj = {
        Id: epi.episodes[i].id,
        SeriesId: curr_tvshow.id,
        Name: gSeason.name,
        SeasonNumber: seasonNumber,
        EpisodeName: epi.episodes[i].name,
        Img: "https://image.tmdb.org/t/p/w500" + epi.episodes[i].still_path,
        Description: epi.episodes[i].overview,
        BroadcastDate: epi.episodes[i].air_date,
    };
    var user_id = JSON.parse(localStorage.getItem('user-login')).Id;
    let api = "../api/Episodes?id=" + user_id;
    ajaxCall("POST", api, JSON.stringify(episodeObj), postEpisodSuccessCB, postEpisodErrorCB);

}

function postEpisodSuccessCB(i) {
    if (i == 1) {
        alert("episod added")
    }
    else {
        alert("something worng / you already added this episod")
    }
}

function postEpisodErrorCB() {
    console.log(err);
}

function getEpisodeErrorCB() {
    console.log(err);
}

function getSeasonSuccessCB(season) {
    gSeason = season;
    renderChat(gSeason);
    renderSeason(season);
   
}

function getSeasonErrorCB(err) {
    console.log(err);
}

function getTVErrorCB(err) {
    console.log(err);
}

// CHAT START

function renderChat(gSeason) {
    initChat(gSeason)
    // const database = firebase.database()
    // database.ref('/series/' + gSeason.name).set({
    //     name: JSON.parse(localStorage.getItem('user-login')).Name,
    //     message: "Hello"
    // });

}


function initChat(){
    active = false;
    msgArr = [];
    chat = firebase.database().ref(gSeason.name);
    reder_messages = document.getElementById("chat-messages");
    // msg = 'hey1'
    // chat.push().set({"msg":msg,"name":JSON.parse(localStorage.getItem('user-login')).Name});
    getMSGfromDB()
    initSendBTN()
    listenToNewMessages()
    // listen to incoming messages
     //listenToNewMessages();
    // listenToNewParticipants();
    // listen to removing messages
    // listenToRemove();
    // ph = document.getElementById("ph");
    // participants = document.getElementById("participants");


}
function listenToNewMessages() {
    // child_added will be evoked for every child that was added
    // on the first entry, it will bring all the childs
    chat.on("child_added", snapshot => {
        msg = {
            name: snapshot.val().name,
            msg: snapshot.val().msg,
        }
        msgArr.push(msg)
        printMessage(msg);
    })
}
function printMessage(msg) {
    let str = `<div class="message">${msg.name}: ${msg.msg}</div>`;
    reder_messages.innerHTML += str;
}

function AddMSG() {
    let msg = $('#chat-input').val()
    let name = JSON.parse(localStorage.getItem('user-login')).Name
    chat.push().set({"msg":msg,"name":name});
    $('#chat-input').val('')
    return
}

function getMSGfromDB() {
    msgArr = [];
    // once listens to an event and then deletes the listner
    // it is usually used to initially bring data
    chat.once("value", snapshot => {
        snapshot.forEach(element => {
            msg = {
            msg:element.val().msg,
            name:element.val().name,
        }
         msgArr.push(msg)
        });
        printMessages(msgArr);
    })

}

function printMessages(msgArr){
    var str = "";
    for (let index = 0; index < msgArr.length; index++) {
        const msg = msgArr[index];
        str += `<div class="message">${msg.name}: ${msg.msg}</div>`
    }
    reder_messages.innerHTML = str;
}
// CHAT END

//Quiz:
//Quiz function:
//return array of string: type of answer, question ,picture ,(API) 4 generate answers
function getQuestions() {
    //there is 4 type of questions:
    //1. birthday of actors
    //2. who's the actor in the picture
    //3. what is the series in the picture
    //4. In which year the series in launch

    //const vars
    let PERSON_NUM = 10000;
    let TV_NUM = 120000;

    //strings
    let url = `https://api.themoviedb.org/3/`;
    let apitoken = `1e5a5ee20af326aebb685a34a1868b76`;
    let urlend = `&language=en-US`;
    // person/tv /<<code>>
    let generateCode;

    //type of q with 4 option
    data = [];

    //generate the type of the question 1-4
    qType = Math.floor(Math.random() * 4) + 1;
    data[0] = qType.toString(); //put type of question	

    switch (qType) {

        //Actors:
        case 1: case 2:
            //1. birthday filed
            //2. profile_path field

            //put type of question	
            if (qType == 1)
                data[1] = 'What is the birthday of actor?';
            else
                data[1] = 'Who is the actor in the picture?';

            //generate 4 answers
            for (let i = 2; i < 6; i++) {
                generateCode = Math.floor(Math.random() * PERSON_NUM) + 1; //generate actor code
                data[i] = url + 'person/' + generateCode + '?api_key=' + apitoken + urlend;
            }
            break;

        //Series:
        case 3: case 4:
            //3. first_air_date field
            //4. poster_path

            //put type of question	
            if (qType == 1)
                data[1] = 'When the series was first to launch?';
            else
                data[1] = 'What is the series in the picrue?';

            //generate 4 answers
            for (let i = 2; i < 6; i++) {
                generateCode = Math.floor(Math.random() * TV_NUM) + 1; //generate actor code
                data[i] = url + 'tv/' + generateCode + '?api_key=' + apitoken + urlend;
            }
            break;

        default:
            console.log(`Sorry, we are out of ${qType}.`);
    }

    console.log(data);

    releaseQuestions(data);
    return;
}

//function get from APIs
//return array of string: type of question, question, 4 answers, number of currect answer 
function releaseQuestions(data) {
    //data = array of string: type of answer, question ,picture ,(API) 4 generate answers

    //there is 4 type of questions:
    //1. birthday of actors - picture, 4 Dates
    //2. who's the actor in the picture - picture, 4 names
    //3. what is the series in the picture - picture, 4 names
    //4. In which year the series in launch - picrue, 4 years(int)

    questionsData = [];
    questionsData[0] = data[0]; //type of question
    questionsData[1] = data[1]; //The question

    console.log(data[2]);
    //put the question picture
    ajaxCall("GET", data[2],null, s(), e()); //id

    /*
    //var ajaxCall1 = $.get(data[2], {}, null);
    console.log(data[2]);
    $.ajax({
        url: data[2],
        //dataType: "JSON",
        type: "GET",
    }).success(function (data1) {

        console.log(data1);
    });
    switch (questionsData[0]) {
        case '1':
            console.log(ajaxCall1.responseJSON.first_air_date.YearBirth);
            break;

        case '2':
            console.log(ajaxCall1.responseJSON.profile_path);
            break;

        case '3':
            console.log(ajaxCall1.responseJSON.poster_path);
            break;

        case '4':
            console.log(ajaxCall1.responseJSON.poster_path);
            break;

        default:
            alert("Error");
    }
    */   

    //put the answer number and generate

    //get the 4 answers

}
function e(e){
    alert(e);
}
function s(obj) {
    console.log(obj);
}