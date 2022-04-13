import React, { useEffect, useState } from "react";
import { addVideo } from "../modules/videoManager";

const VideoForm = ({getVideos}) => {
    const [newVideo, setNewVideo ] = useState({
        title: "",
        url: "",
        description: ""
    });

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
        addVideo(newVideo).then(() => getVideos());
    }

    return (
        <>
            <h3>Upload New Video</h3>
            <form onSubmit={handleSubmit}>
                <label>
                    Title:
                    <input name="title" type="text" onChange={handleUserInput} />
                </label>
                <label>
                    Description:
                    <input name="description" type="text" onChange={handleUserInput} />
                </label>
                <label>
                    Video Url:
                    <input name="url" type="text" onChange={handleUserInput} />
                </label>
                <input type="submit" value="Submit" />
            </form>
        </>
    )
}

export default VideoForm;