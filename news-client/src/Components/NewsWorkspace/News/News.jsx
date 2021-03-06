import React, {Component} from "react";
import './News.scss'
import { NavLink } from "react-router-dom";

export default class News extends Component {
    constructor(){
        super()
        this.state = {
            items: []
        }
    }

    componentDidMount= () => {
        fetch("http://localhost:5295/News/GetNews?page=1", {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            },
        })
            .then((res) => res.json())
            .then((result) => {
                this.setState({
                    isLoaded: true,
                    items: result,
                });
            });
    }

    render(){
        const  news  = this.state.items,
            url = '/NewItem/'

        return(
            <div>
                <p>Список новостей </p>
                <ul>
                    {news.map((new_item) =>
                        <div>
                            <div>
                                <h2><NavLink to={url+new_item.newsId}>{new_item.title}</NavLink></h2>
                                <span>Дата публикации: {new_item.publishDate}</span>
                                <span>Просмотров: {new_item.params.views}</span>
                                <span>Лайков: {new_item.params.likes}</span>
                                <span>Дизлайков: {new_item.params.dislikes}</span>
                            </div>
                        </div>
                    )}
                </ul>
            </div>
        )
    }
}