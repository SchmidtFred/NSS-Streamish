import React, { useEffect, useState } from "react";
import { searchVideos } from "../modules/videoManager";

const VideoSearch = ({setVideos}) => {
    const [searchTerm, setTerm] = useState([]);
    const [sortDesc, setSortDesc] = useState(false);

    const handleUserInput = (event) => {
        if (event.target.id.startsWith("sortDesc")) {
            if (event.target.checked)
            {
                setSortDesc(true);
            }
            else
            {
                setSortDesc(false);
            }
        }
        else
        {
            setTerm(event.target.value);
        }
    }

    const activateSearch = () => {
        searchVideos(searchTerm, sortDesc).then(videos => setVideos(videos));
    }

    return (<>
                <input type="text" onChange={handleUserInput} id='videoSearch' className="form-control" placeholder="Search Videos" />
                <button onClick={activateSearch}>Search</button>
                <label htmlFor="sortDesc">Sort Descending</label>
                <input type="checkbox" onChange={handleUserInput} id='sortDesc' name='sortDesc' />
            </>)
}

export default VideoSearch;