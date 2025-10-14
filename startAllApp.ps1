Write-Host "Cleaning up old containers, networks, and volumes..."
docker compose down -v --remove-orphans

Write-Host "Starting containers frontend and backend..."
docker compose up --build -d

if ($LASTEXITCODE -eq 0) {
    Write-Host "All good! They are alive."
    docker compose ps
} else {
    Write-Host "Failed to start the containers."
}
