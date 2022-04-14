import React, { useState } from "react";
import { Button, Form, FormGroup, Label, Input, FormText } from 'reactstrap';
import { addVideo } from "../modules/videoManager";
import { useHistory } from "react-router-dom";

const VideoForm = () => {
    const [newVideo, setNewVideo ] = useState({
        title: "",
        url: "",
        description: ""
    });
    const history = useHistory();

    const handleUserInput = (event) => {
        const target = event.target; // what is our target
        const value = target.value; // no checkbox
        const name = target.name; // store the name of the changed property
        const copy = {...newVideo}; // copy state
        copy[name] = value; // change the property to the correct value
        setNewVideo(copy); // set it in state
    }
    
    const handleSubmit = (event) => {
        event.preventDefault();
        // add the video to the database and then refresh page by grabbing the new list of videos
        addVideo(newVideo).then((p) => history.push("/"));
    }

    return (
        <>
            <h3>Upload New Video</h3>
            <Form>
                <FormGroup>
                    <Label for="title">Title</Label>
                    <Input name="title" id="title" placeholder="video title" type="text" onChange={handleUserInput} />  
                </FormGroup>  
                <FormGroup>          
                    <Label>Description</Label>
                    <Input name="description" id="description" type="text" onChange={handleUserInput} />  
                </FormGroup>         
                <FormGroup>    
                <Label>Video Url</Label>
                    <Input name="url" id="url" placeholder="video link" type="text" onChange={handleUserInput} />
                </FormGroup>          
                <Button className="btn btn-primary" onClick={handleSubmit}>Submit</Button>
            </Form>
        </>
    )
}

export default VideoForm;