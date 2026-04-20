# This script scrapes Wikipedia pages for The Game Awards from 2014 to 2025
# Created by ChatGPT, modified by Roland Rajnavolgyi
# Used prompt: Provide me a list of game names that has been nominated for any category of The Game Awards since the beginning of that show in json format.
# Dependencies: python, requests, beautifulsoup4

import requests
from bs4 import BeautifulSoup
import json
import re

YEARS = range(2014, 2025)
BASE_URL = "https://en.wikipedia.org/wiki/The_Game_Awards_{}"
HEADERS = {
    "User-Agent": "GameQuizSeeder/1.0 (roland.rajnavolgyi@gmail.com)"
}

games = set()

def clean(text):
    text = re.sub(r"\[.*?\]", "", text)
    return text.strip()

def is_game_candidate(text):
    # filter out obvious non-game entries
    blacklist = [
        "award", "developer", "studio", "esports", "team",
        "league", "player", "coach", "event", "host"
    ]
    lower = text.lower()
    return not any(word in lower for word in blacklist)

for year in YEARS:
    url = BASE_URL.format(year)
    print(f"Processing {year}...")

    res = requests.get(url, headers=HEADERS)
    if res.status_code != 200:
        continue

    soup = BeautifulSoup(res.text, "html.parser")

    # tables contain nominees
    for table in soup.find_all("table"):
        for link in table.find_all("i"):  # game titles are italicized
            name = clean(link.get_text())

            if name and is_game_candidate(name):
                games.add(name)

# sort results
result = sorted(list(games))

with open("game_awards_nominees.json", "w", encoding="utf-8") as f:
    json.dump(result, f, indent=2, ensure_ascii=False)

print(f"Total games: {len(result)}")