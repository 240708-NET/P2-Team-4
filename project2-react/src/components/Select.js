import {React, useState, useEffect} from 'react';
import '../DungeonStyle.css';
import { Routes, useNavigate,  } from 'react-router-dom';

const Select = () => {
  const nav = useNavigate();
  const [cName] = useState([]);

  useEffect(()=> {
    fetch('/getUserByName/${pName}')
    .then((res) => {
      return res.json();
    })
    .then((data) => {
      console.log(data);
      cName(data);
    });
 });
  const goToGame=()=>{
    nav('/Game');
  }
  const goToCreate=()=>{
    nav('/create');
  }
  var pCharacters = [{}]
  return (
    <div>
      <h1>Select</h1>
      <Select options ={cName}/>
      <button onClick={() => goToGame()}>Load Save</button>
      <button onClick={() => goToCreate()}>Create New Character</button>
    </div>
  );
};

export default Select;
