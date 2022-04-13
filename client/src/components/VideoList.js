import React, { useEffect, useState} from "react";
import Video from './Video';
import VideoSearch from "./Search";
import VideoForm from "./VideoForm";
import { getAllVideos } from "../modules/videoManager";

const VideoList = () => {
    const [videos, setVideos] = useState([]);

    const getVideos = () => {
        getAllVideos().then(videos => setVideos(videos));
    };

    useEffect(() => {
        getVideos();
    }, []);

    return (
        <>
            <div>
                <VideoForm getVideos={getVideos}/>
            </div>
            <div>
                <VideoSearch setVideos={setVideos}/>
            </div>
            <div>
                {videos.map(video =>
                    <Video video={video} key={video.id} />
                    )}
            </div>
        </>
    );
}

export default VideoList;